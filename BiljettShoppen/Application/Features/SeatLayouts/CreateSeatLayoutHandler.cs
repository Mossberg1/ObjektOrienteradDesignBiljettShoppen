using DataAccess;
using MediatR;
using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SeatLayouts
{
    public class CreateSeatLayoutHandler : IRequestHandler<CreateSeatLayoutCommand, int>
    {
        private readonly ApplicationDbContext _dbContext;

        public CreateSeatLayoutHandler(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateSeatLayoutCommand request, CancellationToken cancellationToken)
        {
            if (request.NumberOfCols <= 0 || request.NumberOfRows <= 0)
                throw new ArgumentException("Antal rader och kolumner måste vara större än 0");

            var layout = new SeatLayout(
                request.LayoutName, 
                request.NumberOfRows, 
                request.NumberOfCols, 
                request.ArenaId    
            );

            await _dbContext.SeatLayouts.AddAsync(layout, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var seats = request.Seats.Select(s =>
            {
                var price = s.Type == SeatType.Chair ? request.ChairBasePrice : request.BenchBasePrice;
                return new Seat(
                    s.Row,
                    s.Col,
                    s.Type,
                    layout.Id,
                    price,
                    request.EntranceId
                );
            }).ToList();

            await _dbContext.Seats.AddRangeAsync(seats, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return layout.Id;
        }
    }
}
