using MediatR;
using Models.Entities;
using Application.Features.Arenas.GetArenaById;

namespace Application.Features.Arenas.GetArenaById
/// <summary>
/// Query för att hämta en <see cref="Arena"/> baserat på dess ID.
/// <para>
/// Skickas via MediatR till <see cref="GetArenaByIdHandler"/> som returnerar arenan om den finns.
/// </para>
/// </summary>
/// <param name="ArenaId">ID för arenan som ska hämtas (<see cref="Arena"/>).</param>
{
    public record GetArenaByIdQuery(int ArenaId) : IRequest<Arena?>
    {
    }
}
