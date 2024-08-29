using Domain.Entities.CommonClass;
using Domain.Entities;

namespace Domain.Entities
{
    public class ExpenseCategory : DomainObjects
    {
        public TransactionType TransactionTypeId { get; set; }
    }
}

