using Auth.Application.Interfaces.Repositories;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Serilog;
using Shared.Application.Interfaces.Services;
using Shared.Application.Results;
using Shared.Common.Services;
using Shared.Common.Utils;

namespace Auth.Application.Services;

internal sealed class LoginHistoryService(
    ILogger logger,
    ILoginHistoryRepository _repository,
    IDeviceResolver deviceResolver
    )
    : BaseService(logger.ForContext<LoginHistoryService>(), null)
    , ILoginHistoryService
{
    public async Task<Result<bool>> ExistsAsync(int userId, string ip, string userAgent, CancellationToken cancellationToken = default)
    {
        var isSuspicious = await _repository.ExistsAsync(userId, ip, userAgent, cancellationToken).ConfigureAwait(false);

        return Result.Ok(isSuspicious);
    }

    public async Task<Result> LoginAsync(int userId, int sessionId, bool isSuccess, string? failureReason, HttpContext? httpContext, CancellationToken cancellationToken = default)
    {
        var deviceInfo = deviceResolver.Resolve(httpContext);

        var history = new LoginHistory
        {
            UserId = userId,
            SessionId = sessionId,
            IsSuccess = isSuccess,
            FailureReason = failureReason,
            IpAddress = deviceInfo.IpAddress,
            Device = deviceInfo.Device,
            UserAgent = deviceInfo.UserAgent,
            LoginTime = DateTimeUtil.Now
        };

        await _repository.AddAsync(history, cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }

    public async Task<Result> LogoutAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        var history = await _repository.GetBySessionIdAsync(sessionId, cancellationToken).ConfigureAwait(false);

        if (history is null)
            return Result.Fail("Session expired / already logged out."); // session expired / already logged out

        history.LogoutTime = DateTimeUtil.Now;

        await _repository.UpdateAsync(history, cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }
}
