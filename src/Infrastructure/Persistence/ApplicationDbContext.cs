using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private IDbContextTransaction _currentTransaction;

        public ApplicationDbContext(
            DbContextOptions options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }
         
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            var entities = new List<string>();
            var changes = new JArray();
            foreach (var entry in ChangeTracker.Entries<ITrackable>())
            {
                entities.Add(entry.Metadata.DisplayName());
                dynamic change = new JObject
                {
                    ["entity"] = entry.Metadata.DisplayName(),
                    ["state"] = entry.State.ToString()
                };
                dynamic key = new JObject();
                foreach (var k in entry.Metadata.FindPrimaryKey().Properties)
                {
                    key[k.Name] = JToken.FromObject(entry.Member(k.Name).CurrentValue);
                }
                change.key = key;
                if (entry.State == EntityState.Modified)
                {
                    change.properties = JArray.FromObject(entry.Properties.Where(p => p.IsModified).Select(p => new
                    {
                        property = p.Metadata.Name,
                        old = p.OriginalValue,
                        value = p.CurrentValue
                    }).ToList());
                }
                else
                {
                    change.properties = JArray.FromObject(entry.Properties.Select(p => new
                    {
                        property = p.Metadata.Name,
                        value = p.CurrentValue
                    }).ToList());
                }
                changes.Add(change);
            }
            entities = entities.Distinct().ToList();
            if (entities.Count > 0)
            {
                Set<Change>().Add(new Change()
                {
                    Entities = JArray.FromObject(entities).ToString(Newtonsoft.Json.Formatting.Indented),
                    Changes = changes.ToString(Newtonsoft.Json.Formatting.Indented),
                    ChangeBy = _currentUserService.UserId,
                    Changed = _dateTime.Now
                });
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await base.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary keys
            modelBuilder.Entity<ExpenseUserMapping>()
                .HasKey(e => new { e.ApplicationUserId, e.CategoryId });

            modelBuilder.Entity<PayeeListUserMapping>()
                .HasKey(e => new { e.ApplicationUserId, e.PayeeId });

            modelBuilder.Entity<PayementMethodUserMapping>()
                .HasKey(e => new { e.ApplicationUserId, e.PaymentMethodId });
        }
    }
}

