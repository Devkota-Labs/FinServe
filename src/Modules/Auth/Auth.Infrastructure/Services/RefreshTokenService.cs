using Auth.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;
using Shared.Common.Services;
using System.Security.Cryptography;

namespace Auth.Infrastructure.Services;

public sealed class RefreshTokenService : BaseService, IRefreshTokenService
    {
        private readonly AppDbContext _db;
        public RefreshTokenService(ILogger logger, AppDbContext db)
        : base(logger.ForContext<RefreshTokenService>(), null)
    { 
        _db = db; }

        private string GenerateRandomToken(int size = 64) => Convert.ToBase64String(RandomNumberGenerator.GetBytes(size));

        public async Task<RefreshToken> CreateRefreshTokenAsync(int userId, string createdByIp, int days = 30)
        {
            var token = GenerateRandomToken(64);
            var rt = new RefreshToken { UserId = userId, Token = token, CreatedByIp = createdByIp, ExpiresAt = DateTime.UtcNow.AddDays(days) };
            _db.RefreshTokens.Add(rt);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            return rt;
        }

        public async Task<RefreshToken?> GetValidRefreshTokenAsync(string token) =>
            await _db.RefreshTokens.Include(r => r.User).Where(r => r.Token == token && r.RevokedAt == null && r.ExpiresAt > DateTime.UtcNow).FirstOrDefaultAsync().ConfigureAwait(false);

        public async Task RevokeAsync(RefreshToken rt, string? reason = null, string? replacedBy = null)
        {
            rt.RevokedAt = DateTime.UtcNow;
            rt.ReasonRevoked = reason;
            rt.ReplacedByToken = replacedBy;
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
