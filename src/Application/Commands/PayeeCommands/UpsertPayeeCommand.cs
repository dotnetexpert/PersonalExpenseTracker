using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.PayeeCommands
{
    /// <summary>
    /// Adds or updates a payee. If the payee ID is 0, a new payee is added;
    /// otherwise, the existing payee is updated.
    /// </summary>
    public class UpsertPayeeCommand : IRequest<int>
    {
        public PayeeList Payee { get; set; }
    }

    public class UpsertPayeeCommandHandler : IRequestHandler<UpsertPayeeCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpsertPayeeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpsertPayeeCommand request, CancellationToken cancellationToken)
        {
            if (request.Payee == null) return 0;

            if (request.Payee.Id == 0)
            {
                await _context.Set<PayeeList>().AddAsync(request.Payee, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var mapping = new PayeeListUserMapping
                {
                    ApplicationUserId = request.Payee.ApplicationUserId,
                    PayeeId = request.Payee.Id
                };

                await _context.Set<PayeeListUserMapping>().AddAsync(mapping, cancellationToken);
            }
            else
            {
                _context.Set<PayeeList>().Update(request.Payee);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return 1;
        }
    }
}
