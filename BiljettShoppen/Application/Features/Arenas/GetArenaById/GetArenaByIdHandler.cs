using DataAccess;
using MediatR;
using Models.Entities;
using Application.Features.Arenas.GetArenaById;

namespace Application.Features.Arenas.GetArenaById
/// <summary>
/// Hanterar hämtning av en <see cref="Arena"/> baserat på dess ID.
/// <para>
/// Tar emot <see cref="GetArenaByIdQuery"/> via MediatR och returnerar arenan om den finns.
/// </para>
/// </summary>
{
    public class GetArenaByIdHandler : IRequestHandler<GetArenaByIdQuery, Arena?>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetArenaByIdHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Arena?> Handle(GetArenaByIdQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Arenas.FindAsync(request.ArenaId);
        }
    }
}
