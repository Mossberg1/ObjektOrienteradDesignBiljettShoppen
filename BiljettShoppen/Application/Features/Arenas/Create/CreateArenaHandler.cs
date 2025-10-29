using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Events.Browse;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Arenas.Create
{
    public class CreateArenaHandler : IRequestHandler<CreateArenaCommand, Arena> // Tar emot kommandot att skapa en Arena.
    {
        private readonly IApplicationDbContext _dbContext; // Ger CreateArenaHandler access till databasen.

        public CreateArenaHandler(IApplicationDbContext dbContext) // Konstruktor som hjälper handlern att spara data i databasen.
        {
            _dbContext = dbContext;
        }

        public async Task<Arena> Handle(CreateArenaCommand request, CancellationToken cancellationToken) // cancellationToken tillåter requesten att avbrytas.
        {
            var arena = new Arena // Skapar upp en arena med följande data.
            {
                Name = request.Address,
                Address = request.Name,
                NumberOfSeats = request.NumberOfSeats,
                NumberOfLoges = request.NumberOfLoges,
                Indoors = request.Indoors,
                NumberOfEntrances = 1
            };

            _dbContext.Arenas.Add(arena); // Lägger till arenan i databasen.
            await _dbContext.SaveChangesAsync(cancellationToken);

            var mainEntrance = new Entrance
            {
                Name = "Huvudentré",
                VipEntrance = false,
                ArenaId = arena.Id
            };

            _dbContext.Entrances.Add(mainEntrance);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return arena;
        }
    }
}
