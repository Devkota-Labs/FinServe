using Auth.Application.Interfaces.Repositories;
using Auth.Domain.Entities;
using Auth.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

internal sealed class LoginHistoryRepository(AuthDbContext db) : ILoginHistoryRepository
{
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<LoginHistory?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<LoginHistory?> GetBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        return await db.LoginHistories
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.SessionId == sessionId, cancellationToken)
           .ConfigureAwait(false);
    }

    public async Task AddAsync(LoginHistory loginHistory, CancellationToken cancellationToken = default)
    {
        await db.LoginHistories.AddAsync(loginHistory, cancellationToken).ConfigureAwait(false);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(LoginHistory token, CancellationToken cancellationToken = default)
    {
        db.LoginHistories.Update(token);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(LoginHistory token, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
