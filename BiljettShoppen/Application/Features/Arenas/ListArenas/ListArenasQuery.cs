using MediatR;
using Models.Entities;
using Application.Features.Arenas;

namespace Application.Features.Arenas
/// <summary>
/// Query för att hämta alla <see cref="Arena"/>-posts.
/// <para>
/// Skickas via MediatR till <see cref="ListArenasHandler"/> som returnerar en lista med alla arenor i databasen.
/// </para>
/// </summary>
{
    public record ListArenasQuery() : IRequest<List<Arena>>;
}
