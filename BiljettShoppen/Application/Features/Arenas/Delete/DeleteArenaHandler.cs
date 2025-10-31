using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
using Application.Features.Arenas.Delete;

namespace Application.Features.Arenas.Delete
    /// <summary>
    /// Hanterar borttagning av en arena från databasen.
    /// <para>
    /// Tar emot ett <see cref="DeleteArenaCommand"/> via MediatR och tar bort
    /// motsvarande arena om den finns. Om arenan inte hittas kastas en <see cref="IdNotFoundException"/>.
    /// </para>
    /// </summary>
{
    public class DeleteArenaHandler : IRequestHandler<DeleteArenaCommand, bool>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteArenaHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteArenaCommand request, CancellationToken cancellationToken)
        {

            var arena = await _dbContext.Arenas
                .FirstOrDefaultAsync(a => a.Id == request.ArenaId, cancellationToken);

            if (arena == null)
                throw new IdNotFoundException($"Arena med ID {request.ArenaId} hittades inte.");

            _dbContext.Arenas.Remove(arena);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
