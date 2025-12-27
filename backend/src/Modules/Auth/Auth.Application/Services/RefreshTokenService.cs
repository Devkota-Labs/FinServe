using Auth.Application.Interfaces.Repositories;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Serilog;
using Shared.Common.Services;
using Shared.Common.Utils;
using System.Security.Cryptography;

namespace Auth.Application.Services;
internal sealed class RefreshTokenService(ILogger logger, IRefreshTokenRepository refreshTokenRepository)
    : BaseService(logger.ForContext<RefreshTokenService>(), null), IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

    private static string GenerateRandomToken(int size = 64) => Convert.ToBase64String(RandomNumberGenerator.GetBytes(size));

    public async Task<RefreshToken> CreateRefreshTokenAsync(int userId, string createdByIp, int expiryMinutes = 30, CancellationToken cancellationToken = default)
    {
        var token = GenerateRandomToken(64);

        var prt = new RefreshToken
        {
            UserId = userId,
            Token = token,
            CreatedAt = DateTimeUtil.Now,
            CreatedByIp = createdByIp,
            ExpiresAt = DateTimeUtil.Now.AddMinutes(expiryMinutes),
            IsUsed = false
        };

        await _refreshTokenRepository.AddAsync(prt, cancellationToken).ConfigureAwait(false);
        return prt;
    }

    public async Task<RefreshToken?> GetValidRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _refreshTokenRepository.GetByTokenAsync(token, cancellationToken).ConfigureAwait(false);
    }

    public async Task RevokeAsync(RefreshToken rt, string? reason = null, string? replacedBy = null, CancellationToken cancellationToken = default)
    {
        rt.RevokedAt = DateTimeUtil.Now;
        rt.ReasonRevoked = reason;
        rt.ReplacedByToken = replacedBy;
        rt.IsUsed = true;

        await _refreshTokenRepository.UpdateAsync(rt, cancellationToken).ConfigureAwait(false);
    }
    public async Task RevokeAllAsync(int userId, CancellationToken cancellationToken = default)
    {
        await _refreshTokenRepository.RevokeAllAsync(userId, cancellationToken).ConfigureAwait(false);
    }
}
