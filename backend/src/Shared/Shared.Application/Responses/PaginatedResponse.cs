using Shared.Application.Results;

namespace Shared.Application.Responses;

public class PaginatedResponse : ApiResponse
{
    public PaginationMetadata Pagination { get; set; } = null!;

    public static PaginatedResponse FromPaginatedResult<T>(PaginatedResult<T> r)
    {
        return new PaginatedResponse
        {
            Success = r.Success,
            Message = r.Message,
            Code = r.ErrorCode,
            Data = r.Data,
            Errors = r.Errors,
            Pagination = new PaginationMetadata
            {
                Page = r.Page,
                PageSize = r.PageSize,
                TotalRecords = r.TotalRecords,
                TotalPages = r.TotalPages
            }
        };
    }
}
