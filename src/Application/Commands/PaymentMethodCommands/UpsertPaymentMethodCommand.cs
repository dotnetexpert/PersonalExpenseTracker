using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.PaymentMethodCommands
{
    /// <summary>
    /// Adds or updates a payment method. If the payment method ID is 0, a new payment method is added;
    /// otherwise, the existing payment method is updated.
    /// </summary>
    /// <param name="paymentMethod">The payment method object to be added or updated.</param>
    /// <returns>The added or updated payment method.</returns>
    public class UpsertPaymentMethodCommand : IRequest<int>
    {
        public PaymentMethod PaymentMethod { get; set; }
    }

    public class UpsertPaymentMethodCommandHandler : IRequestHandler<UpsertPaymentMethodCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpsertPaymentMethodCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpsertPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            if (request.PaymentMethod == null) return 0;

            if (request.PaymentMethod.Id == 0)
            {
                await _context.Set<PaymentMethod>().AddAsync(request.PaymentMethod, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var mapping = new PayementMethodUserMapping
                {
                    ApplicationUserId = request.PaymentMethod.ApplicationUserId,
                    PaymentMethodId = request.PaymentMethod.Id
                };

                await _context.Set<PayementMethodUserMapping>().AddAsync(mapping, cancellationToken);
            }
            else
            {
                _context.Set<PaymentMethod>().Update(request.PaymentMethod);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return 1;
        }
    }
}
