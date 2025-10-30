using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Exceptions;

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
                throw new IdNotFoundException($"Arena med Id {request.ArenaId} hittades inte.");
            }

            var seatLayout = arena.SeatLayoutsNavigation.FirstOrDefault();

            if (seatLayout == null)
            {
                throw new IdNotFoundException($"Ingen stol för arenan ({arena.Name}) hittades.");
            }

            return arena.SeatLayoutsNavigation;
        }
    }
}