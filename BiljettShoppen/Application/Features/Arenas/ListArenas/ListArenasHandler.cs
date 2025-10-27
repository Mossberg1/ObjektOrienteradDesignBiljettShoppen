using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Arenas
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
