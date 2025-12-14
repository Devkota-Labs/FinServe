namespace Shared.Application.Results;

public class PaginatedResult<T> : Result<List<T>>
{
    public int Page { get; }
    public int PageSize { get; }
    public int TotalRecords { get; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

    private PaginatedResult(List<T> data, int page, int pageSize, int totalRecords, string message)
        : base(true, message, null, data, null)
    {
        Page = page;
        PageSize = pageSize;
        TotalRecords = totalRecords;
    }

    public static PaginatedResult<T> Create(List<T> data, int page, int pageSize, int totalRecords, string message = null)
        => new PaginatedResult<T>(data, page, pageSize, totalRecords, message ?? "Data fetched successfully");
}
