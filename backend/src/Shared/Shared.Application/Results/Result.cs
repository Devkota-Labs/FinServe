namespace Shared.Application.Results;

public class Result
{
    public bool Success { get; }
    public string? Message { get; }
    public string? ErrorCode { get; }
    public ICollection<ValidationError> Errors { get; private set; }

    protected Result(bool success, string? message, string? errorCode, ICollection<ValidationError>? errors)
    {
        Success = success;
        Message = message;
        ErrorCode = errorCode;
        Errors = errors ?? [];
    }

    public static Result Ok(string? message = null)
        => new(true, message, null, null);

    public static Result<T> Ok<T>(string message, T? data)
        => new(true, message, null, data, null);

    public static Result<T> Ok<T>(T data)
        => new(true, null, null, data, null);

    //public static Result<T> Ok<T>(string? message, T data)
    //    => new(true, message, null, data, null);

    public static Result Fail(string? message = null)
        => new(false, message, null, null);

    public static Result<T> Fail<T>(string? message)
        => new(false, message, null, default, null);

    public static Result Fail(string? message, string? errorCode = null)
        => new(false, message, errorCode, null);

    public static Result<T> Fail<T>(string? message, T data, string? code = null)
        => new(false, message, null, data, null);

    //public static Result<T> Fail(string? message, string? code = null)
    //    => new(false, message, code, default, null);

    public static Result Validation(ICollection<ValidationError> errors)
        => new(false, "Validation failed.", "VALIDATION_ERROR", errors);
    public static Result<T> Validation<T>(ICollection<ValidationError>? errors)
        => new(false, "Validation failed.", "VALIDATION_ERROR", default, errors);

    public static PaginatedResult<T> Create<T>(ICollection<T> data, int page, int pageSize, int totalRecords, string? message = null)
        => new(data, page, pageSize, totalRecords, message ?? "Data fetched successfully");
}
