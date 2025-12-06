using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Shared.Logging;

public class RequestLoggingMiddleware(RequestDelegate next, ILogProvider logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogProvider _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var sw = Stopwatch.StartNew();
        _logger.Information($"Request {context.Request.Method} {context.Request.Path} started");
        await _next(context).ConfigureAwait(false);
        sw.Stop();
        _logger.Information($"Request {context.Request.Method} {context.Request.Path} finished in {sw.ElapsedMilliseconds} ms");
    }
}
