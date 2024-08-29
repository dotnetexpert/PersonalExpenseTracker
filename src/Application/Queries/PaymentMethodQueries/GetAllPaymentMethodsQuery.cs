using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.PaymentMethodQueries
{
    /// <summary>
    /// Retrieves all payment methods associated with a specific user.
    /// Combines both user-defined and default payment methods.
    /// </summary>
    /// <param name="ApplicationUserId">The ID of the application user.</param>
    /// <returns>A list of combined user-defined and default payment methods.</returns>
    public class GetAllPaymentMethodsQuery : IRequest<PaymentMethodArray>
    {
        public string ApplicationUserId { get; set; }
    }

    public class GetAllPaymentMethodsQueryHandler : IRequestHandler<GetAllPaymentMethodsQuery, PaymentMethodArray>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPaymentMethodsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentMethodArray> Handle(GetAllPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            var paymentMethods = await _context.Set<PayementMethodUserMapping>()
                .Where(m => m.ApplicationUserId == request.ApplicationUserId && !m.PaymentMethod.isDeleted)
                .Include(m => m.PaymentMethod)
                .Select(m => m.PaymentMethod)
                .ToListAsync(cancellationToken);

            var defaultCategory = await _context.Set<PaymentMethod>()
                .Where(m => m.IsDefault && !m.isDeleted)
                .ToListAsync(cancellationToken);

            PaymentMethodArray paymentMethodArray = new PaymentMethodArray();
                paymentMethodArray.PaymentMethods = defaultCategory.Union(paymentMethods).ToList();

            return paymentMethodArray;
        }
    }
}
