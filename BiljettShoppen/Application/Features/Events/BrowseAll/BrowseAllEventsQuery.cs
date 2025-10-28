using DataAccess.Utils;
using MediatR;
using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
