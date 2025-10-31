using DataAccess.Utils;
using MediatR;
using Models.Entities;
using Models.Enums;

namespace Application.Features.Events.Browse;
/// <summary>
/// Query för att hämta en lista av <see cref="Event"/> med släppta biljetter.
/// <para>
/// Endast events med ReleaseTicketsDate <= DateTime.UtcNow inkluderas.
/// </para>
/// </summary>
/// <param name="SearchWord">Sökord för att filtrera events baserat på namn, arena eller adress.</param>
/// <param name="Type">Filtrerar events efter <see cref="EventType"/>.</param>
/// <param name="IsFamilyFriendly">Filtrerar events efter om de är familjevänliga (true) eller inte (false).</param>
/// <param name="FromDate">Filtrerar events som sker från valt datum och framåt.</param>
/// <param name="ToDate">Filtrerar events som sker fram till valt datum.</param>
/// <param name="SortBy">Anger fält för sortering, t.ex. "name" eller "startdate".</param>
/// <param name="Ascending">Anger om sorteringen ska vara stigande (true) eller fallande (false).</param>
/// <param name="PageNumber">Sidan som ska hämtas.</param>
/// <param name="PageSize">Antal events per sida.</param>

public record BrowseReleasedEventsQuery(
    string? SearchWord,
    EventType? Type,
    bool? IsFamilyFriendly,
    DateOnly? FromDate,
    DateOnly? ToDate,
    string? SortBy,
    bool Ascending = true,
    int PageNumber = 1,
    int PageSize = 24
) : IRequest<PaginatedList<Event>>;