using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Exceptions;
using Application.Features.Arenas.GetSeatLayoutsForArena;

namespace Application.Features.Arenas.GetSeatLayoutsForArena
/// <summary>
/// Hanterar hämtning av alla <see cref="SeatLayout"/> för en specifik <see cref="Arena"/>.
/// <para>
/// Tar emot <see cref="GetSeatLayoutForArenaIdQuery"/> via MediatR, kontrollerar att arenan och stolslayouten finns <br/>
/// och returnerar en lista med alla stolslayouter för arenan.
/// </para>
/// </summary>

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