namespace Shared.Kernel;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; init; } = new List<T>();
    public int TotalCount { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }

    public PagedResult() { }
    public PagedResult(IEnumerable<T> items, int total, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = total;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
