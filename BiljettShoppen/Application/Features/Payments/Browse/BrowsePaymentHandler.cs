using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Features.Payments.PayBooking;

namespace Application.Features.Payments.PayBooking
/// <summary>
/// Handler för <see cref="BrowsePaymentQuery"/>.
/// <para>
/// Hämtar alla betalningar från databasen och returnerar en lista av payments"/>.
/// </para>
/// </summary>
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
