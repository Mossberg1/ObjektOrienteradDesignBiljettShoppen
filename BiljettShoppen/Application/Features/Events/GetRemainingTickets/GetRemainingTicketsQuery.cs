using MediatR;

namespace Application.Features.Events.GetRemainingTickets
{
    public record GetRemainingTicketsQuery(int EventId) : IRequest<RemainingTickets>;
}