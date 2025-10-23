using MediatR;
using Models.Entities;

namespace Application.Features.Tickets.CreateTickets;

public record CreateTicketsCommand(int EventId) : IRequest<bool>;