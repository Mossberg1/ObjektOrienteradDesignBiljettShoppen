using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Arenas.GetSeatLayoutsForArena

{

    public class GetSeatLayoutForArenaIdQueryHandler : IRequestHandler<GetSeatLayoutForArenaIdQuery, List<SeatLayout>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetSeatLayoutForArenaIdQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SeatLayout>> Handle(GetSeatLayoutForArenaIdQuery request, CancellationToken cancellationToken)
        {
            var arena = await _dbContext.Arenas
                .AsNoTracking()
                .Include(a => a.SeatLayoutsNavigation)
                .FirstOrDefaultAsync(a => a.Id == request.ArenaId, cancellationToken);

            if (arena == null)
            {
                throw new Exception($"Arena with Id {request.ArenaId} not found.");
            }

            var seatLayout = arena.SeatLayoutsNavigation.FirstOrDefault();

            if (seatLayout == null)
            {
                throw new Exception($"No Seat Layout found for Arena {arena.Name}.");
            }

            return arena.SeatLayoutsNavigation;
        }
    }
}