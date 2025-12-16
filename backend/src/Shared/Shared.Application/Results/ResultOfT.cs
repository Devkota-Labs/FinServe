namespace Shared.Application.Results;

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
