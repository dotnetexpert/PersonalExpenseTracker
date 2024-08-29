using Domain.Entities;
using System.Collections.Generic;

namespace Application.Queries.PaymentMethodQueries
{
    public class PaymentMethodArray
    {
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }
    }
}
