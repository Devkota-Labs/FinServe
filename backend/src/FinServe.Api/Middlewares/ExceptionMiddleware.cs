using Shared.Application.Responses;
using Shared.Application.Results;
using Shared.Common.Services;
using System.Text.Json;

namespace FinServe.Api.Middlewares;

internal sealed class ExceptionMiddleware(Serilog.ILogger logger, RequestDelegate next)
    : BaseService(logger.ForContext<ExceptionMiddleware>(), null)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            await next(httpContext).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Unhandled exception");
            await HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        if (context.Response.HasStarted)
            throw ex;

        var result = Result.Fail("An unexpected error occurred.", "SERVER_ERROR");
        var response = ApiResponse.FromResult(result);

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response)).ConfigureAwait(false);
    }
}