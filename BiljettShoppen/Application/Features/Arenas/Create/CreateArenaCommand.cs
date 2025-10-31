using MediatR;
using Models.Entities;


namespace Application.Features.Arenas.Create;
/// <summary>
/// Ett kommando som används för att skapa en ny <see cref=Arena"/>.
/// </summary>
/// <remarks>
/// Denna request skickas genom MediatR och returnerar den skapade arenan.
/// </remarks>
/// <param name="Name">Arenans namn.</param>
/// <param name="Address">Arenans adress.</param>
/// <param name="NumberOfSeats">Antalet sittplatser i arenan.</param>
/// <param name="NumberOfLoges">Antalet loger i arenan.</param>
/// <param name="Indoors">Anger om arenan är inomhus (true) eller utomhus (false).</param>
public record CreateArenaCommand(
    string Name,
    string Address,
    int NumberOfSeats,
    int NumberOfLoges,
    bool Indoors
) : IRequest<Arena>;

