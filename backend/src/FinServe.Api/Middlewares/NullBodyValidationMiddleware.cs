using Shared.Application.Responses;
using Shared.Application.Results;

namespace FinServe.Api.Middlewares;

internal sealed class NullBodyValidationMiddleware(RequestDelegate next)
{
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
                var response = ApiResponse.FromResult(Result.Fail("Request body cannot be null or empty."));
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(response).ConfigureAwait(false);
                return;
            }
        }

        await next(context).ConfigureAwait(false);
    }
}
