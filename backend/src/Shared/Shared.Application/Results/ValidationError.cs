namespace Shared.Application.Results;

public sealed class ValidationError
{
    public string Field { get; }
    public string Message { get; }

    public ValidationError(string field, string message)
    {
        Field = field;
        Message = message;
    }
}

public class Result
{
    public bool Success { get; }
    public string? Message { get; }
    public string? ErrorCode { get; }
    public ICollection<ValidationError> Errors { get; }

    protected Result(bool success, string? message, string? errorCode, ICollection<ValidationError>? errors)
    {
        Success = success;
        Message = message;
        ErrorCode = errorCode;
        Errors = errors ?? [];
    }

    public static Result Ok(string? message = null)
        => new(true, message, null, null);

    public static Result Fail(string? message, string? errorCode = null)
        => new(false, message, errorCode, null);

    public static Result Validation(ICollection<ValidationError> errors)
        => new(false, "Validation failed.", "VALIDATION_ERROR", errors);
}

public class Result<T> : Result
{
    public T? Data { get; }

    protected Result(bool success, string? message, string? code, T? data, ICollection<ValidationError>? errors)
        : base(success, message, code, errors)
    {
        Data = data;
    }

    public static Result<T> Ok(T data)
        => new(true, null, null, data, null);

    public static Result<T> Ok(string? message, T data)
        => new(true, message, null, data, null);

    public static Result<T> Fail(string? message, T? data = default, string? code = null)
        => new(false, message, null, data, null);

    //public static Result<T> Fail(string? message, string? code = null)
    //    => new(false, message, code, default, null);

    public static Result<T> Validation(ICollection<ValidationError>? errors)
        => new(false, "Validation failed.", "VALIDATION_ERROR", default, errors);
}

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
