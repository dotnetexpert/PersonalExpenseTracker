using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.PaymentMethodCommands
{
    /// <summary>
    /// Soft deletes a payment method by marking it as deleted.
    /// </summary>
    /// <param name="id">The ID of the payment method to delete.</param>
    /// <returns>A success or failure message.</returns>
    public class DeletePaymentMethodCommand : IRequest<int>
    {
        public int PaymentMethodId { get; set; }
    }

    public class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeletePaymentMethodCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var paymentMethodFromDb = await _context.Set<PaymentMethod>().FindAsync(request.PaymentMethodId);
            if (paymentMethodFromDb == null) return 0;

            paymentMethodFromDb.isDeleted = true;
            _context.Set<PaymentMethod>().Update(paymentMethodFromDb);

            await _context.SaveChangesAsync(cancellationToken);
            return 1;
        }
    }
}
