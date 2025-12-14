namespace Shared.Application.Results;

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
