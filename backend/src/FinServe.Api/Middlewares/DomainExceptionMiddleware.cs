using Shared.Application.Exceptions;
using Shared.Application.Responses;
using Shared.Application.Results;
using System.Net;
using System.Text.Json;

namespace FinServe.Api.Middlewares;

public sealed class DomainExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public DomainExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (DomainException dex)
        {
            if (context.Response.HasStarted)
                throw;

            var result = Result.Fail(dex.Message, dex.Code);
            var apiResponse = ApiResponse.FromResult(result);

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(apiResponse)).ConfigureAwait(false);
        }
    }
}
