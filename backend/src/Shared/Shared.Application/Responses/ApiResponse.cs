using Shared.Application.Results;

namespace Shared.Application.Responses;

public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; } = null!;
    public string? Code { get; set; } = null!;
    public object? Data { get; set; } = null!;
    public ICollection<ValidationError> Errors { get; private set; } = null!;

    protected void AddErrors(ICollection<ValidationError> errors)
    {
        Errors = errors;
    }
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
