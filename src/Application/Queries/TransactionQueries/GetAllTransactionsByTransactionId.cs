using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.TransactionQueries
{
    //        /// <summary>
    //        /// Retrieves all transactions for a specific user, grouped by various time periods (today, last 7 days, this month, this year).
    //        /// </summary>
    //        /// <param name="userId">The ID of the user whose transactions are to be retrieved.</param>
    //        /// <returns>A summary of transactions for the specified user.</returns>
    public class GetAllTransactionsByTransactionId : IRequest<GetAllTransactionsByTransactionIdVM>
    {
        public string UserId { get; set; }
    }
    public class GetAllTransactionsByTransactionIdHandler : IRequestHandler<GetAllTransactionsByTransactionId, GetAllTransactionsByTransactionIdVM>
    {
        private readonly IApplicationDbContext _context;
        public GetAllTransactionsByTransactionIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<GetAllTransactionsByTransactionIdVM> Handle(GetAllTransactionsByTransactionId request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                return null;
            }
            //Getting transaction details on the basis of ApplicationUserId with properties of Other Tables
            var transactions = await _context.Set<Transaction>().Where(t => t.ApplicationUserId == request.UserId).Include(m => m.PayeeList).Include(m => m.ExpenseCategory).Include(m => m.PaymentMethod).ToListAsync();
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);
            var last7Days = today.AddDays(-7);
            //Filtering Data on the basis of date 
            var todayTransactions = transactions.Where(t => t.TransactionDate.Date == today);
            var last7DaysTransactions = transactions.Where(t => t.TransactionDate >= last7Days && t.TransactionDate < today.AddDays(1));
            var thisMonthTransactions = transactions.Where(t => t.TransactionDate >= startOfMonth && t.TransactionDate < startOfMonth.AddMonths(1));
            var thisYearTransactions = transactions.Where(t => t.TransactionDate >= startOfYear && t.TransactionDate < startOfYear.AddYears(1));
            return new GetAllTransactionsByTransactionIdVM
            {
                Today = Summary.CalculateSummary(todayTransactions),
                Last7Days = Summary.CalculateSummary(last7DaysTransactions),
                ThisMonth = Summary.CalculateSummary(thisMonthTransactions),
                ThisYear = Summary.CalculateSummary(thisYearTransactions)
            };
        }
    }

    /// <summary>
    //        /// Calculates the summary of a set of transactions, including total income, total expense, and outstanding balance.
    //        /// </summary>
    //        /// <param name="transactions">The list of transactions to summarize.</param>
    //        /// <returns>A summary object containing income, expenses, and transaction details.</returns>

    public static class Summary
    {
        public static TransactionSummaryVM CalculateSummary(IEnumerable<Transaction> transactions)
        {
            var totalIncome = transactions.Where(t => t.TransactionTypeId == TransactionType.Credit).Sum(t => t.ExpenseAmount);
            var totalExpense = transactions.Where(t => t.TransactionTypeId == TransactionType.Debit).Sum(t => t.ExpenseAmount);
            var outstandingBalance = totalIncome - totalExpense;

            var transactionDetails = transactions.Select(t => new TransactionDetailVM()
            {
                TransactionId = t.TransactionId,
                ExpenseAmount = t.ExpenseAmount,
                TransactionDate = t.TransactionDate,
                Description = t.Description,
                TransactionType = t.TransactionTypeId == 0 ? "Debit" : "Credit",
                ExpenseCategory = new ExpenseCategoryVM()
                {
                    CategoryId = t.CategoryId,
                    Name = t.ExpenseCategory.Name
                },
                Payee = new PayeeVM()
                {
                   PayeeId = t.PayeeId,
                   Name = t.PayeeList.Name
                },
                PaymentMethod = new PaymentMethodVM()
                {
                    PaymentMethodId = t.PaymentMethodId,
                    Name = t.PaymentMethod.Name
                }
            }).ToList();

            return new TransactionSummaryVM()
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                OutstandingBalance = outstandingBalance,
                Transactions = transactionDetails
            };
        }
    }
}
