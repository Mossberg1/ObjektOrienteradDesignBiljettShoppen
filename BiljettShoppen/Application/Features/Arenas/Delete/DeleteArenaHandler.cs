using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Arenas.Delete
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
                throw new KeyNotFoundException($"Arena with ID {request.ArenaId} not found.");
            
            _dbContext.Arenas.Remove(arena);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
