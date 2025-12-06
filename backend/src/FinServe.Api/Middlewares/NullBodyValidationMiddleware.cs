using Shared.Application.Responses;
using System.Net;

namespace FinServe.Api.Middlewares;

internal sealed class NullBodyValidationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // Only validate JSON POST/PUT/PATCH
        if (context.Request.ContentType?.Contains("application/json", StringComparison.CurrentCulture) == true &&
            (context.Request.Method == HttpMethods.Post ||
             context.Request.Method == HttpMethods.Put ||
             context.Request.Method == HttpMethods.Patch))
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);
            context.Request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(body))
            {
                var response = new ApiResponse<string>(HttpStatusCode.BadRequest, "Request body cannot be null or empty.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(response).ConfigureAwait(false);
                return;
            }
        }

        await _next(context).ConfigureAwait(false);
    }
}
