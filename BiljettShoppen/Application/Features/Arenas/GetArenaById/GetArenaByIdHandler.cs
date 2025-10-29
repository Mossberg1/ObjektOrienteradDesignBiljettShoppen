using DataAccess;
using MediatR;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Arenas.GetArenaById
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
