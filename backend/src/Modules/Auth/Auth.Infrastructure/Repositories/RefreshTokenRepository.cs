using Auth.Application.Interfaces.Repositories;
using Auth.Domain.Entities;
using Auth.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

internal sealed class RefreshTokenRepository(AuthDbContext db) : IRefreshTokenRepository
{
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await db.RefreshTokens.AnyAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<RefreshToken?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await db.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await db.RefreshTokens
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Token == token && !x.IsUsed && x.RevokedAt == null && x.ExpiresAt > DateTime.UtcNow, cancellationToken)
                        .ConfigureAwait(false);
    }

    public async Task<ICollection<RefreshToken>?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await db.RefreshTokens
                        .Where(x => x.UserId == userId)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        // Invalidate old tokens
        var existing = await db.RefreshTokens
            .Where(x => x.UserId == token.UserId && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        foreach (var t in existing)
        {
            t.IsUsed = true;
        }

        await db.RefreshTokens.AddAsync(token, cancellationToken).ConfigureAwait(false);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        //_db.RefreshTokens.Update(token);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        db.RefreshTokens.Remove(token);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
