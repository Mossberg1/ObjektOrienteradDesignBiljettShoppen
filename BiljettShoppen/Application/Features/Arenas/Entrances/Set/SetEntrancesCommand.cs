using MediatR;
using Models.Entities;
using Application.Features.Arenas.Entrances.Set;

namespace Application.Features.Arenas.Entrances.Set
/// <summary>
/// Detta kommando används för att uppdatera antalet entréer för en befintlig <see cref="Arena"/>.
/// <para>
/// Skickas via MediatR till <see cref="SetEntrancesHandler"/> som uppdaterar arenans entréer i databasen.
/// </para>
/// </summary>
/// <param name="ArenaId">ID för arenan som ska uppdateras (<see cref="Arena"/>).</param>
/// <param name="NumberOfEntrances">Totala antalet entréer.</param>
{
    public record SetEntrancesCommand(
        int ArenaId,
        int NumberOfEntrances
        ) : IRequest<Arena>;
}
