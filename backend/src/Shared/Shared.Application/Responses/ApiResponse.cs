using Shared.Application.Results;

namespace Shared.Application.Responses;

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Code { get; set; }
    public object Data { get; set; }
    public ICollection<ValidationError> Errors { get; set; }

    public static ApiResponse FromResult(Result r)
    {
        return new ApiResponse
        {
            Success = r.Success,
            Message = r.Message,
            Code = r.ErrorCode,
            Errors = r.Errors
        };
    }

    public static ApiResponse FromResult<T>(Result<T> r)
    {
        return new ApiResponse
        {
            Success = r.Success,
            Message = r.Message,
            Code = r.ErrorCode,
            Errors = r.Errors,
            Data = r.Data
        };
    }
}

public class PaginatedResponse : ApiResponse
{
    public PaginationMetadata Pagination { get; set; }

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

public class PaginationMetadata
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
}