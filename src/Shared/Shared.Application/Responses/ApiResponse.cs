using System.Net;

namespace Shared.Application.Responses;

public sealed class ApiResponse<T>(HttpStatusCode statusCode, string? message = null, T? data = null) where T : class
{
    public int StatusCode { get; } = (int)statusCode;
    public HttpStatusCode HttpStatusCode { get; } = statusCode;
    public string? Message { get; } = message;
    public T? Data { get; } = data;
}
