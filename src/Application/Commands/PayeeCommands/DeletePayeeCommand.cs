using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.PayeeCommands
{
    /// <summary>
    /// Soft deletes a payee by marking it as deleted.
    /// </summary>
    public class DeletePayeeCommand : IRequest<int>
    {
        public int PayeeId { get; set; }
    }

    public class DeletePayeeCommandHandler : IRequestHandler<DeletePayeeCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeletePayeeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeletePayeeCommand request, CancellationToken cancellationToken)
        {
            var payeeFromDb = await _context.Set<PayeeList>().FindAsync(request.PayeeId);
            if (payeeFromDb == null) return 0;

            payeeFromDb.isDeleted = true;
            _context.Set<PayeeList>().Update(payeeFromDb);

            await _context.SaveChangesAsync(cancellationToken);
            return 1;
        }
    }
}
