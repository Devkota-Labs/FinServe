using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Services;

public interface IRefreshTokenService
{
    Task<RefreshToken> CreateRefreshTokenAsync(int userId, string createdByIp, int expiryMinutes = 30, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetValidRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
    Task RevokeAsync(RefreshToken rt, string? reason = null, string? replacedBy = null, CancellationToken cancellationToken = default);
    Task RevokeAllAsync(int userId, CancellationToken cancellationToken = default);
}
