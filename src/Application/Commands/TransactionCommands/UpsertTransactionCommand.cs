using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.TransactionCommands
{
    //        /// <summary>
    //        /// Adds or updates a transaction based on whether it already exists.
    //        /// </summary>
    //        /// <param name="transaction">The transaction to be added or updated.</param>
    //        /// <returns>The added or updated transaction.</returns>
    public class UpsertTransactionCommand : IRequest<int>
    {
        public Transaction Transaction { get; set; }
    }

    public class UpsertTransactionCommandHandler : IRequestHandler<UpsertTransactionCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpsertTransactionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpsertTransactionCommand request, CancellationToken cancellationToken)
        {
            if (request.Transaction == null) return 0;
            if (request.Transaction.TransactionId == 0)
                await _context.Set<Transaction>().AddAsync(request.Transaction, cancellationToken);
            else
                _context.Set<Transaction>().Update(request.Transaction);
            await _context.SaveChangesAsync(cancellationToken);
            return 1;
        }
    }
}
