namespace Shared.Application.Results;

public class Result<T> : Result
{
    public T? Data { get; }

    public Result(bool success, string? message, string? code, T? data, ICollection<ValidationError>? errors)
        : base(success, message, code, errors)
    {
        Data = data;
    }
}
