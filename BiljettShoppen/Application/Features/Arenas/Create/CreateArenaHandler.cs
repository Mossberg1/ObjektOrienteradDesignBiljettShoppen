using DataAccess.Interfaces;
using MediatR;
using Models.Entities;

namespace Application.Features.Arenas.Create
/// <summary>
/// Hanterar skapandet av en ny <see cref=Arena"/>.
/// <para>
/// Tar emot <see cref="CreateArenaCommand"/> via MediatR, sparar den i databasen <br/>
/// och lägger till en huvudentré.
/// </para>
/// </summary>
{
    public class CreateArenaHandler : IRequestHandler<CreateArenaCommand, Arena>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateArenaHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Arena> Handle(CreateArenaCommand request, CancellationToken cancellationToken)
        {
            var arena = new Arena
            {
                Name = request.Name,
                Address = request.Address,
                NumberOfSeats = request.NumberOfSeats,
                NumberOfLoges = request.NumberOfLoges,
                Indoors = request.Indoors,
                NumberOfEntrances = 1
            };

            _dbContext.Arenas.Add(arena);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var mainEntrance = new Entrance
            {
                Name = "Huvudentré",
                VipEntrance = false,
                ArenaId = arena.Id
            };

            _dbContext.Entrances.Add(mainEntrance);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return arena;
        }
    }
}
