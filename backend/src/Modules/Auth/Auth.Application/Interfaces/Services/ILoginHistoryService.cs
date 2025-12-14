using Microsoft.AspNetCore.Http;
using Shared.Application.Results;

namespace Auth.Application.Interfaces.Services;

public interface ILoginHistoryService
{
    Task<Result> LoginAsync(int userId, int sessionId, bool isSuccess, string? failureReason, HttpContext httpContext, CancellationToken cancellationToken = default);
    Task<Result> LogoutAsync(int sessionId, CancellationToken cancellationToken = default);
}