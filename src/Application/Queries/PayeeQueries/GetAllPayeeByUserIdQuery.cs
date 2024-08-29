using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.PayeeQueries
{
    /// <summary>
    /// Retrieves all payees associated with a specific user.
    /// Combines both user-defined and default payees based on the transaction type (Debit or Credit).
    /// </summary>
    public class GetAllPayeeByUserIdQuery : IRequest<PayeeListArray>
    {
        public string ApplicationUserId { get; set; }
        public int TransactionId { get; set; }
    }

    public class GetAllPayeeByUserIdQueryHandler : IRequestHandler<GetAllPayeeByUserIdQuery, PayeeListArray>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPayeeByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PayeeListArray> Handle(GetAllPayeeByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (request.TransactionId > 1)
            {
                return null;
            }

            var payeeList = await _context.Set<PayeeListUserMapping>()
                .Where(m => m.ApplicationUserId == request.ApplicationUserId && !m.PayeeList.isDeleted && m.PayeeList.TransactionTypeId == Enum.Parse<TransactionType>(request.TransactionId.ToString()))
                .Include(m => m.PayeeList)
                .Select(m => m.PayeeList)
                .ToListAsync(cancellationToken);

            var defaultCategory = await _context.Set<PayeeList>()
                .Where(m => m.IsDefault && !m.isDeleted && m.TransactionTypeId == Enum.Parse<TransactionType>(request.TransactionId.ToString()))
                .ToListAsync(cancellationToken);

            PayeeListArray payeeListArray = new PayeeListArray();
            payeeListArray.PayeeLists = defaultCategory.Union(payeeList).ToList();
            return payeeListArray;
        }
    }
}
