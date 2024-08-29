using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.TransactionCommands
{

    //        /// <summary>
    //        /// Deletes a transaction based on its ID.
    //        /// </summary>
    //        /// <param name="id">The ID of the transaction to be deleted.</param>
    //        /// <returns>A success or failure message based on the outcome.</returns>

    public class DeleteTransactionByIdCommand : IRequest<int>
    {
        public int TransactionId { get; set; }
    }

    public class DeleteTransactionByIdCommandHandler : IRequestHandler<DeleteTransactionByIdCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTransactionByIdCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteTransactionByIdCommand request, CancellationToken cancellationToken)
        {
            var paymentMethodFromDb = await _context.Set<Transaction>().FindAsync(request.TransactionId);
            if (paymentMethodFromDb == null) return 0;
            _context.Set<Transaction>().Remove(paymentMethodFromDb);
            await _context.SaveChangesAsync(cancellationToken);
            return 1;
        }
    }
}
