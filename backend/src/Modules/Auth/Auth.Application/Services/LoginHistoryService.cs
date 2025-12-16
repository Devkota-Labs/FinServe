using Auth.Application.Interfaces.Repositories;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;

namespace Auth.Application.Services;

internal sealed class LoginHistoryService(ILogger logger, ILoginHistoryRepository _repository)
    : BaseService(logger.ForContext<LoginHistoryService>(), null), ILoginHistoryService
{
    private static string GetIpAddress(HttpContext context)
    {
        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    private static string GetUserAgent(HttpContext context)
    {
        return context.Request.Headers.UserAgent.ToString();
    }

    public async Task<Result> LoginAsync(int userId, int sessionId, bool isSuccess, string? failureReason, HttpContext httpContext, CancellationToken cancellationToken = default)
    {
        var history = new LoginHistory
        {
            UserId = userId,
            SessionId = sessionId,
            IsSuccess = isSuccess,
            FailureReason = failureReason,
            IpAddress = GetIpAddress(httpContext),
            Device = GetUserAgent(httpContext),
            LoginTime = DateTime.UtcNow
        };

        await _repository.AddAsync(history, cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }

    public async Task<Result> LogoutAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        var history = await _repository.GetBySessionIdAsync(sessionId, cancellationToken).ConfigureAwait(false);

        if (history is null)
            return Result.Fail("Session expired / already logged out."); // session expired / already logged out

        history.LogoutTime = DateTime.UtcNow;

        await _repository.UpdateAsync(history, cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }
}
