using System.Net;

namespace Shared.Application.Responses;

public abstract class BaseResponse(HttpStatusCode statusCode, string? message = null)
{
    public int StatusCode { get; } = (int)statusCode;
    public HttpStatusCode HttpStatusCode { get; } = statusCode;
    public string? Message { get; } = message;
}
