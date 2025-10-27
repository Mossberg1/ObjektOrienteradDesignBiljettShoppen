using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using MediatR;
using Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Arenas

{

    public class GetSeatLayoutForArenaIdQueryHandler : IRequestHandler<GetSeatLayoutForArenaIdQuery, SeatLayoutDto>
    {
        private readonly IApplicationDbContext _dbContext;
        public GetSeatLayoutForArenaIdQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SeatLayoutDto> Handle(GetSeatLayoutForArenaIdQuery request, CancellationToken cancellationToken)
        {
            var arena = await _dbContext.Arenas
                .AsNoTracking()
                .Include(a => a.SeatLayoutsNavigation)
                .ThenInclude(sl => sl.SeatsNavigation)
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

            return new SeatLayoutDto
            {
                Id = seatLayout.Id,
                ArenaId = seatLayout.ArenaId,
                NumberOfRows = seatLayout.NumberOfRows,
                NumberOfCols = seatLayout.NumberOfCols,
                TotalCapacity = seatLayout.SeatsNavigation.Count
            };
        }
    }
}