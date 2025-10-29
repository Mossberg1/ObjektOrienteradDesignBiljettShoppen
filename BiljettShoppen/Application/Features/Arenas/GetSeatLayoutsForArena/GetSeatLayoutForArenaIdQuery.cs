using MediatR;
using Models.Entities;

namespace Application.Features.Arenas.GetSeatLayoutsForArena
{
    public record GetSeatLayoutForArenaIdQuery(int ArenaId) : IRequest<List<SeatLayout>>;
}


