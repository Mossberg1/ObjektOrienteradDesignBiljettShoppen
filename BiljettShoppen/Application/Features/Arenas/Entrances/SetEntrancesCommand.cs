using MediatR;
using Models.Entities;

namespace Application.Features.Arenas.Entrances
{
    public record SetEntrancesCommand(
        int ArenaId,
        int NumberOfEntrances
        ) : IRequest<Arena>;
}
