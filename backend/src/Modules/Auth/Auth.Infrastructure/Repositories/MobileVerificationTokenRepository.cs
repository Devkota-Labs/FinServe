//using Auth.Domain.Entities;
//using Auth.Infrastructure.Db;

//namespace Auth.Infrastructure.Repositories;

//internal sealed class MobileVerificationTokenRepository(AuthDbContext db) : IMobileVerificationTokenRepository
//{
//    private readonly AuthDbContext _db = db;

//    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
//    {
//        return await _db.MobileVerificationTokens.AnyAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
//    }

//    public async Task<MobileVerificationToken?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
//    {
//        return await _db.MobileVerificationTokens
//            .AsNoTracking()
//            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
//            .ConfigureAwait(false);
//    }

//    public async Task<MobileVerificationToken?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
//    {
//        return await _db.MobileVerificationTokens
//                        .Where(x => x.UserId == userId && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow)
//                        .OrderByDescending(x => x.Id)
//                        .AsNoTracking()
//                        .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
//    }

//    public async Task AddAsync(MobileVerificationToken token, CancellationToken cancellationToken = default)
//    {
//        // Invalidate old tokens
//        var existing = await _db.MobileVerificationTokens
//            .Where(x => x.UserId == token.UserId && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow)
//            .ToListAsync(cancellationToken).ConfigureAwait(false);

//        foreach (var t in existing)
//        {
//            t.IsUsed = true;
//        }

//        await _db.MobileVerificationTokens.AddAsync(token, cancellationToken).ConfigureAwait(false);
//        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
//    }

//    public async Task UpdateAsync(MobileVerificationToken token, CancellationToken cancellationToken = default)
//    {
//        _db.MobileVerificationTokens.Update(token);
//        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
//    }

//    public async Task DeleteAsync(MobileVerificationToken token, CancellationToken cancellationToken = default)
//    {
//        _db.MobileVerificationTokens.Remove(token);
//        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
//    }
//}
