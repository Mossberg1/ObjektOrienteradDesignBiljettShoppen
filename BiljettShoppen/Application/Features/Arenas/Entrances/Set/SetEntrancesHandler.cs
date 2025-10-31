using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Exceptions;
using Application.Features.Arenas.Entrances.Set;

namespace Application.Features.Arenas.Entrances.Set
/// <summary>
/// Hanterar uppdatering av antalet entréer för en befintlig <see cref="Arena"/>.
/// <para>
/// Tar emot <see cref="SetEntrancesCommand"/> via MediatR, kontrollerar att arenan finns <br/>
/// och uppdaterar antalet entréer för att spara ändringen i databasen.
/// </para>
/// </summary>
{
    public class SetEntrancesHandler : IRequestHandler<SetEntrancesCommand, Arena>
    {
        private readonly IApplicationDbContext _dbContext;

        public SetEntrancesHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Arena> Handle(SetEntrancesCommand request, CancellationToken cancellationToken)
        {
            var arena = await _dbContext.Arenas
                .FirstOrDefaultAsync(a => a.Id == request.ArenaId, cancellationToken);

            if (arena == null)
                throw new IdNotFoundException($"Arena med ID {request.ArenaId} hittades inte.");

            arena.NumberOfEntrances = request.NumberOfEntrances;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return arena;
        }
    }
}
