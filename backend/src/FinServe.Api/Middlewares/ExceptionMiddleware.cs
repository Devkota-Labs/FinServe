using Serilog;
using Shared.Application.Responses;
using System.Net;
using System.Text.Json;

namespace FinServe.Api.Middlewares;

internal sealed class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private readonly Serilog.ILogger _logger = Log.ForContext<ExceptionMiddleware>();

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception");
            await HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = HttpStatusCode.InternalServerError;

        var response = new ApiResponse<string>(
            statusCode,
            "An unexpected error occurred",
            ex.Message
        );

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json).ConfigureAwait(false);
    }
}
