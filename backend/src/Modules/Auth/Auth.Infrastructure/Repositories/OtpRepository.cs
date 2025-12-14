using Auth.Application.Interfaces.Repositories;
using Auth.Domain.Entities;
using Auth.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Utils;

namespace Auth.Infrastructure.Repositories;

internal sealed class OtpRepository(AuthDbContext db) : IOtpRepository
{
    private readonly AuthDbContext _db = db;

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<OtpVerification?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<OtpVerification?> GetActiveAsync(int userId, string token, OtpPurpose purpose, CancellationToken cancellationToken = default)
    {
        return await _db.OtpVerifications
                        .Where(x => x.UserId == userId && x.ConsumedAt == null && x.ExpiresAt >= DateTime.UtcNow && x.Token == token && x.Purpose == purpose)
                        .AsNoTracking()
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task AddAsync(OtpVerification token, CancellationToken cancellationToken = default)
    {
        // Invalidate old tokens
        var existing = await _db.OtpVerifications
            .Where(x => x.UserId == token.UserId 
            && x.ConsumedAt == null
            && x.ExpiresAt >= DateTime.UtcNow
            )
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        foreach (var t in existing)
        {
            t.ConsumedAt = DateTimeUtil.Now;
        }

        await _db.OtpVerifications.AddAsync(token, cancellationToken).ConfigureAwait(false);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(OtpVerification token, CancellationToken cancellationToken = default)
    {
        _db.OtpVerifications.Update(token);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(OtpVerification token, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
