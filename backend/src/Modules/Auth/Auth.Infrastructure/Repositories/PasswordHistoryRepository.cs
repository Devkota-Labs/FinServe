using Auth.Application.Interfaces.Repositories;
using Auth.Domain.Entities;
using Auth.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

internal sealed class PasswordHistoryRepository(AuthDbContext authDbContext) : IPasswordHistoryRepository
{
    private readonly AuthDbContext _db = authDbContext;

    public async Task<List<PasswordHistory>> GetRecentAsync(int userId, int limit, CancellationToken ct)
    {
        return  await _db.PasswordHistories
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Id)
            .Take(limit)
            .ToListAsync(ct).ConfigureAwait(false);
    }

    public async Task AddAsync(PasswordHistory history, CancellationToken ct)
    {
        _db.PasswordHistories.Add(history);
        await _db.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
