using MediatR;

namespace Application.Features.Events.Delete;


public record DeleteEventCommand(int EventId) : IRequest<bool>;
