using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.ExpenseCategoryQueries
{
    /// <summary>
    /// Retrieves all expense categories associated with a specific user.
    /// Combines both user-defined and default categories based on the transaction type (Debit or Credit).
    /// </summary>
    /// <param name="ApplicationUserId">The ID of the application user.</param>
    /// <param name="transId">Transaction type ID (0 for Debit, 1 for Credit).</param>
    /// <returns>A list of combined user-defined and default categories.</returns>
    public class GetAllExpenseCategoryByUserId : IRequest<ExpenseCategoryArray>
    {
        public string ApplicationUserId { get; set; }
        public int TransactionId { get; set; }
    }
    public class GetAllExpenseCategoryByUserIdHandler : IRequestHandler<GetAllExpenseCategoryByUserId, ExpenseCategoryArray>
    {
        private readonly IApplicationDbContext _context;
        public GetAllExpenseCategoryByUserIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ExpenseCategoryArray> Handle(GetAllExpenseCategoryByUserId request, CancellationToken cancellationToken)
        {
            if (request.TransactionId > 1)
            {
                return null;
            }
            ExpenseCategoryArray expenseCategoryArray = new ExpenseCategoryArray();
            //Getting Categories from mapping(ExpenseUserMappings) and master(ExpenseCategory)  table of related ApplicationUserid and transactionTypeId
            var categoryList = _context.Set<ExpenseUserMapping>()
            .Where(m => m.ApplicationUserId == request.ApplicationUserId && !m.ExpenseCategory.isDeleted && m.ExpenseCategory.TransactionTypeId == Enum.Parse<TransactionType>(request.TransactionId.ToString()))
            .Include(m => m.ExpenseCategory).Select(m => m.ExpenseCategory).ToList();
            //Getting Default Categories from Master(ExpenseCategory) Table on basis of transactiontype ,isdefault and isdeleted
            var defaultCategory = _context.Set<ExpenseCategory>().Where(m => m.IsDefault && !m.isDeleted && m.TransactionTypeId == Enum.Parse<TransactionType>(request.TransactionId.ToString())).ToList();
            //Combinig the both default and userdefined Categories
            expenseCategoryArray.ExpenseCategories = defaultCategory.Union(categoryList).ToList();

            return expenseCategoryArray;
        }
    }
}
