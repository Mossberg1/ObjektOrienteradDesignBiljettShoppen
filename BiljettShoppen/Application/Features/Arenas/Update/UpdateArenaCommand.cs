using MediatR;
using Models.Entities;

namespace Application.Features.Arenas.Update;
/// <summary>
/// Används för att uppdatera en befintlig <see cref="Arena"/>.
/// <para>
/// Skickas via MediatR till UpdateArenaHandler som uppdaterar arenan i databasen.
/// </para>
/// </summary>
/// <param name="Id">ID för arenan som ska uppdateras (<see cref="Arena"/>).</param>
/// <param name="Name">Namn på arenan.</param>
/// <param name="Address">Adress för arenan.</param>
/// <param name="Indoors">Anger om arenan är inomhus (true) eller utomhus (false).</param>

public record UpdateArenaCommand(
    int Id,
    string Name,
    string Address,
    bool Indoors
    ) : IRequest<Arena?>;
