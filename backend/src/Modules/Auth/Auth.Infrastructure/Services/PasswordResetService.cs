//using Auth.Domain.Entities;
//using Auth.Infrastructure.Db;
//using System.Security.Cryptography;

//namespace Auth.Infrastructure.Services;

//internal sealed class PasswordResetService : IPasswordResetService
//{
//    private readonly AuthDbContext _db;

//    public PasswordResetService(AuthDbContext db)
//    {
//        _db = db;
//    }

//    public async Task<PasswordResetToken> CreateTokenAsync(int userId, int expiryMinutes = 30)
//    {
//        // Invalidate old tokens
//        var existing = await _db.PasswordResetTokens
//            .Where(x => x.UserId == userId && !x.Used && x.ExpiresAt > DateTime.UtcNow)
//            .ToListAsync().ConfigureAwait(false);

//        foreach (var t in existing)
//        {
//            t.Used = true;
//        }

//        var tokenBytes = RandomNumberGenerator.GetBytes(32);
//        var token = Convert.ToBase64String(tokenBytes);

//        var prt = new PasswordResetToken
//        {
//            UserId = userId,
//            Token = token,
//            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
//            Used = false
//        };

//        _db.PasswordResetTokens.Add(prt);
//        await _db.SaveChangesAsync().ConfigureAwait(false);
//        return prt;
//    }

//    public async Task<User?> ValidateTokenAsync(string token)
//    {
//        var record = await _db.PasswordResetTokens
//            .Include(p => p.User)
//            .FirstOrDefaultAsync(p => p.Token == token && !p.Used && p.ExpiresAt > DateTime.UtcNow).ConfigureAwait(false);

//        if (record == null) return null;

//        record.Used = true;
//        await _db.SaveChangesAsync().ConfigureAwait(false);
//        return record.User;
//    }
//}
