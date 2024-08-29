using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.ExpenseCategoryCommands
{
    //        /// <summary>
    //        /// Soft deletes an expense category by marking it as deleted.
    //        /// </summary>
    //        /// <param name="id">The ID of the expense category to delete.</param>
    //        /// <returns>A success or failure message.</returns>
    public class DeleteExpenseCategoryCommand : IRequest<int>
    {
        public int ExpenseCategoryId { get; set; }
    }
    public class DeleteExpenseCategoryCommandHandler : IRequestHandler<DeleteExpenseCategoryCommand, int>
    {
        private readonly IApplicationDbContext _context;
        public DeleteExpenseCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryFromDb = await _context.Set<ExpenseCategory>().FindAsync(request.ExpenseCategoryId);
            if (categoryFromDb == null)
                return 0;
            categoryFromDb.isDeleted = true;
            _context.Set<ExpenseCategory>().Update(categoryFromDb);
            await _context.SaveChangesAsync(cancellationToken);
            return 1;
        }
    }
}
