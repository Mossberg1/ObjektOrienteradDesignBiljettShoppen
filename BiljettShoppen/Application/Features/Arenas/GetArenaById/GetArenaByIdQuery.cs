using MediatR;
using Models.Entities;

namespace Application.Features.Arenas.GetArenaById
{
    public record GetArenaByIdQuery(int ArenaId) : IRequest<Arena?>
    {
    }
}
