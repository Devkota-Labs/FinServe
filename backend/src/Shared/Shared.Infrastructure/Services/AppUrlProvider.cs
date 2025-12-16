using Microsoft.AspNetCore.Http;
using Serilog;
using Shared.Application.Interfaces.Services;
using Shared.Common.Services;

namespace Shared.Infrastructure.Services;

internal sealed class AppUrlProvider(ILogger logger, IHttpContextAccessor httpContextAccessor)
    : BaseService(logger.ForContext<AppUrlProvider>(), null), IAppUrlProvider
{
    public Uri GetBaseUrl()
    {
        var request = httpContextAccessor.HttpContext?.Request;

        if (request == null)
            return new Uri(""); // Background workers may call this

        return new Uri($"{request.Scheme}://{request.Host}");
    }
}
