using Domain.Entities;
using System.Collections.Generic;

namespace Application.Queries.TransactionQueries
{
    public class TransactionVM
    {
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
