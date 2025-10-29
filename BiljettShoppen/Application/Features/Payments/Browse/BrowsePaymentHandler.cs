using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Payments.PayBooking
{
    public class BrowsePaymentHandler : IRequestHandler<BrowsePaymentQuery, List<Models.Entities.Payment>>
    {
        private readonly IApplicationDbContext _dbContext;

        public BrowsePaymentHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Models.Entities.Payment>> Handle(BrowsePaymentQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Payments.ToListAsync(cancellationToken);
        }
    }
}
