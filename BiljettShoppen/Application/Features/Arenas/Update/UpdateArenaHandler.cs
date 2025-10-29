using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Arenas.Update;

public class UpdateArenaHandler : IRequestHandler<UpdateArenaCommand, Arena?>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateArenaHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Arena?> Handle(UpdateArenaCommand request, CancellationToken cancellationToken)
    {
        var arena = await _dbContext.Arenas
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (arena == null)
            return null;

        arena.Name = request.Name;
        arena.Address = request.Address;
        arena.Indoors = request.Indoors;

        if (await _dbContext.SaveChangesAsync(cancellationToken) == 0)
            return null;

        return arena;

    }
}
