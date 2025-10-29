using DataAccess.Utils;
using MediatR;
using Models.Entities;
using Models.Enums;

namespace Application.Features.Events.BrowseAll
{
    public record BrowseAllEventsQuery(
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
}
