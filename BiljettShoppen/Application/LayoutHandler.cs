using System;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
// ... (Dina using-satser)

namespace Application.Handlers
{
    // OBS: IRequestHandler<TRequest> utan returtyp
    public class UpdateSeatLayoutGridHandler : IRequestHandler<UpdateSeatLayoutGridCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateSeatLayoutGridHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // OBS: Returnerar bara Task, inte Task<Unit>
        public async Task Handle(UpdateSeatLayoutGridCommand request, CancellationToken cancellationToken)
        {
            // Validering
            if (request.NewNumberOfRows <= 0 || request.NewNumberOfCols <= 0)
            {
                throw new ArgumentException("Antal rader och kolumner måste vara positiva tal.");
            }

            var seatLayout = await _dbContext.SeatLayouts
                .FirstOrDefaultAsync(l => l.Id == request.SeatLayoutId, cancellationToken);

            if (seatLayout == null)
            {
                throw new Exception($"Säteslayout med ID {request.SeatLayoutId} hittades inte.");
            }

            seatLayout.NumberOfRows = request.NewNumberOfRows;
            seatLayout.NumberOfCols = request.NewNumberOfCols;

            await _dbContext.SaveChangesAsync(cancellationToken);

        }

       
    }
}


