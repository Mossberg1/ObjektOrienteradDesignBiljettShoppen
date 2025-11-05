using Microsoft.EntityFrameworkCore;

namespace DataAccess.Utils;

/// <summary>
/// Datastruktur för att hämta in paginerade listor från databasen.
/// Innehåller information om aktuell sida, total antal sidor, total antal objekt.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public int PageSize { get; }

    public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public int FirstItemIndex => TotalCount == 0 ? 0 : (PageNumber - 1) * PageSize + 1;
    public int LastItemIndex => Math.Min(PageNumber * PageSize, TotalCount);

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> queryable, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var count = await queryable.CountAsync(cancellationToken);

        var items = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}