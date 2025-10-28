using MediatR;
using Models.Entities;

namespace Application.Features.Events.GetById;

public record GetEventByIdQuery(int EventId) : IRequest<Event?>;