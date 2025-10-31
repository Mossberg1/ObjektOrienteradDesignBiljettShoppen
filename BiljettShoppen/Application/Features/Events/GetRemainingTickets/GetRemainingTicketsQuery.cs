using MediatR;
using Application.Features.Events.GetRemainingTickets;

namespace Application.Features.Events.GetRemainingTickets
/// <summary>
/// En MediatR request som används för att hämta information <br/>
/// om antalet tillgängliga biljetter för ett specifikt evenemang.
/// </summary>
/// <param name="EventId">
/// Unikt ID för evenemanget.
/// </param>
/// <remarks>
/// Denna query hanteras av <see cref="RemainingTicketsHandler"/> 
/// och returnerar ett <see cref="RemainingTickets"/>-objekt som innehåller <br/>
/// information om totalt antal, sålda och kvarvarande biljetter.
/// </remarks>
{
    public record GetRemainingTicketsQuery(int EventId) : IRequest<RemainingTickets>;
}