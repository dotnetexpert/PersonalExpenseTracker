using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.ExpenseCategoryCommands
{
    //        /// <summary>
    //        /// Adds or updates an expense category. If the category ID is 0, a new category is added;
    //        /// otherwise, the existing category is updated.
    //        /// </summary>
    //        /// <param name="category">The expense category object to be added or updated.</param>
    //        /// <returns>The added or updated expense category.</returns>
    public class UpsertExpenseCategory : IRequest<int>
    {
        public ExpenseCategory ExpenseCategory { get; set; }
    }
    public class UpsertExpenseCategoryHandler : IRequestHandler<UpsertExpenseCategory, int>
    {
        private readonly IApplicationDbContext _context;
        public UpsertExpenseCategoryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(UpsertExpenseCategory request, CancellationToken cancellationToken)
        {
            if (request.ExpenseCategory == null) return 0;
            {
                if (request.ExpenseCategory.Id == 0)
                {
                    await _context.Set<ExpenseCategory>().AddAsync(request.ExpenseCategory);
                    await _context.SaveChangesAsync(cancellationToken);
                    var mapping = new ExpenseUserMapping
                    {
                        ApplicationUserId = request.ExpenseCategory.ApplicationUserId,
                        CategoryId = request.ExpenseCategory.Id,
                    };
                    await _context.Set<ExpenseUserMapping>().AddAsync(mapping);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                    _context.Set<ExpenseCategory>().Update(request.ExpenseCategory);

                await _context.SaveChangesAsync(cancellationToken);
                return 1;
            }
        }
    }
}
