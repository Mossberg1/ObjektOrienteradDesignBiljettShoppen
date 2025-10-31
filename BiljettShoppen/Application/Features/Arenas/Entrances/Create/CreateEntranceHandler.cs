using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Exceptions;
using Application.Features.Arenas.Entrances.Create;

namespace Application.Features.Arenas.Entrances.Create
/// <summary>
/// Hanterar skapandet av en ny <see cref="Entrance"/> för en specifik arena.
/// <para>
/// Tar emot <see cref="CreateEntranceCommand"/> via MediatR, kontrollerar att arenan finns <br/>
/// och skapar entrén för att spara den i databasen.
/// </para>
/// </summary>
{
    public class CreateEntranceHandler : IRequestHandler<CreateEntranceCommand, Entrance>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateEntranceHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Entrance> Handle(CreateEntranceCommand request, CancellationToken cancellationToken)
        {
            var arenaExists = await _dbContext.Arenas
                .AnyAsync(a => a.Id == request.ArenaId, cancellationToken);

            if (arenaExists)
                throw new IdNotFoundException($"Arena med ID {request.ArenaId} hittades inte.");

            var entrance = new Entrance
            {
                Name = request.Name,
                VipEntrance = request.VipEntrance,
                ArenaId = request.ArenaId
            };

            _dbContext.Entrances.Add(entrance);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entrance;
        }
    }
}
