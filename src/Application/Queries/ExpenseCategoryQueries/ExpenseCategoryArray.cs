using Domain.Entities;
using System.Collections.Generic;

namespace Application.Queries.ExpenseCategoryQueries
{
    public class ExpenseCategoryArray
    {
        public IEnumerable<ExpenseCategory> ExpenseCategories { get; set; }
    }
}
