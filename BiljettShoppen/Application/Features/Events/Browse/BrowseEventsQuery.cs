using DataAccess.Utils;
using MediatR;
using Models.Entities;
using Models.Enums;

namespace Application.Features.Events.Browse;

public record BrowseEventsQuery(
    string? SearchWord,
    EventType? Type,
    DateOnly? FromDate, 
    DateOnly? ToDate,
    int PageNumber = 1,
    int PageSize = 24
) : IRequest<PaginatedList<Event>>;