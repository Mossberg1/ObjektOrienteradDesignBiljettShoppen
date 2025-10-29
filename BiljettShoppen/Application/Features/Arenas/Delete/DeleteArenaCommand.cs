using MediatR;

namespace Application.Features.Arenas.Delete
{
    public record DeleteArenaCommand(int ArenaId) : IRequest<bool>;
}
