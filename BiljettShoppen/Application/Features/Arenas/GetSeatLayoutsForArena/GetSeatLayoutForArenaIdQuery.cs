using MediatR;
using Models.Entities;
using Application.Features.Arenas.GetSeatLayoutsForArena;

namespace Application.Features.Arenas.GetSeatLayoutsForArena
/// <summary>
/// Query för att hämta alla <see cref="SeatLayout"/> för en specifik <see cref="Arena"/>.
/// <para>
/// Skickas via MediatR till <see cref="GetSeatLayoutForArenaIdQueryHandler"/> som returnerar en lista med alla stolslayouter för arenan.
/// </para>
/// </summary>
{
    public record GetSeatLayoutForArenaIdQuery(int ArenaId) : IRequest<List<SeatLayout>>;
}


