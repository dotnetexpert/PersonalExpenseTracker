using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.TransactionQueries
{
    /// <summary>
    /// Retrieves transactions for a specific user over a specified time period.
    /// </summary>
    /// <param name="userId">The ID of the user whose transactions are to be retrieved.</param>
    /// <param name="startDate">The start date of the period.</param>
    /// <param name="endDate">The end date of the period.</param>
    /// <returns>A summary of transactions for the specified period.</returns>
    public class GetTransactionsForPeriod : IRequest<TransactionSummaryVM>
    {
        public string UserId { get; set; }
        public DateTime StartDate {  get; set; }
        public DateTime EndDate { get; set; }
    }
    public class GetTransactionsForPeriodHandler : IRequestHandler<GetTransactionsForPeriod, TransactionSummaryVM>
    {
        private readonly IApplicationDbContext _context;
        public GetTransactionsForPeriodHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TransactionSummaryVM> Handle(GetTransactionsForPeriod request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                return null;
            }

            var transactions = await _context.Set<Transaction>().Where(t => t.ApplicationUserId == request.UserId && t.TransactionDate >= request.StartDate && t.TransactionDate < request.EndDate).Include(m => m.PayeeList).Include(m => m.ExpenseCategory).Include(m => m.PaymentMethod).ToListAsync();

            return Summary.CalculateSummary(transactions);
        }
    }
}
