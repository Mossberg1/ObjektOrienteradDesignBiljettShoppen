using MediatR;
using Models.Entities;

namespace Application.Features.Events.Browse;

public record BrowseEventsQuery() : IRequest<List<Event>>;