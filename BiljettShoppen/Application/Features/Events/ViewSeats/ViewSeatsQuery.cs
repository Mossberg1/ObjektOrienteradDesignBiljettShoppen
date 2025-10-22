using MediatR;
using Models.Entities;

namespace Application.Features.Events.ViewSeats;

public record ViewSeatsQuery(int EventId) : IRequest<Event?>;