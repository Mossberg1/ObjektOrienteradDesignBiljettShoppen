using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Exceptions;

namespace Application.Features.Arenas.Entrances.Set
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
