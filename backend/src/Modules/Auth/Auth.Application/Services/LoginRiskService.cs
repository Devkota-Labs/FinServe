using Auth.Application.Interfaces.Services;
using Serilog;
using Shared.Common.Services;

namespace Auth.Application.Services;

internal sealed class LoginRiskService(
    ILogger logger,
    ILoginHistoryService loginHistoryService
    )
    : BaseService(logger.ForContext<LoginRiskService>(), null)
    , ILoginRiskService
{
    public async Task<bool> IsSuspiciousAsync(int userId, string ip, string userAgent, CancellationToken cancellationToken = default)
    {
        var serviceResponse = await loginHistoryService.ExistsAsync(userId, ip, userAgent, cancellationToken).ConfigureAwait(false);

        return !serviceResponse.Data;
    }
}
