namespace Shared.Application.Results;

public class PaginatedResult<T> : Result<ICollection<T>>
{
    public int Page { get; }
    public int PageSize { get; }
    public int TotalRecords { get; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

    public PaginatedResult(ICollection<T> data, int page, int pageSize, int totalRecords, string message)
        : base(true, message, null, data, null)
    {
        Page = page;
        PageSize = pageSize;
        TotalRecords = totalRecords;
    }
}
