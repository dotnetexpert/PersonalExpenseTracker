using Domain.Entities;
using System.Collections.Generic;

namespace Application.Queries.PayeeQueries
{
    public class PayeeListArray
    {
        public IEnumerable<PayeeList> PayeeLists { get; set; }
    }
}
