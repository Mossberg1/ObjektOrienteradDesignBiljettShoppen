using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Application.Features.Arenas;

namespace Application.Features.Arenas
/// <summary>
/// Hanterar hämtning av alla <see cref="Arena"/>-posts.
/// <para>
/// Tar emot <see cref="ListArenasQuery"/> via MediatR och returnerar en lista med alla arenor i databasen.
/// </para>
/// </summary>
{
    public class ListArenasHandler : IRequestHandler<ListArenasQuery, List<Arena>>
    {
        private readonly IApplicationDbContext _dbContext;

        public ListArenasHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Arena>> Handle(ListArenasQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Arenas
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
