using MediatR;
using Models.Entities;

namespace Application.Features.Arenas.Entrances.Set
{
    public record SetEntrancesCommand(
        int ArenaId,
        int NumberOfEntrances
        ) : IRequest<Arena>;
}
