namespace Shared.Common;

public record PaginationQuery(int PageNumber = 1, int PageSize = 20)
{
    public int Skip => (PageNumber - 1) * PageSize;
}
