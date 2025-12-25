using Microsoft.EntityFrameworkCore;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Infrastructure.Db;

namespace Notification.Infrastructure.Repositories;

internal sealed class UserNotificationRepository(NotificationDbContext db) : IUserNotificationRepository
{
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await db.UserNotifications.AnyAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<UserNotification>> GetByUserAsync(int userId, int take, CancellationToken cancellationToken = default)
    {
        return await db.UserNotifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> GetUnreadCountAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await db.UserNotifications
            .CountAsync(x => x.UserId == userId && !x.IsRead, cancellationToken).ConfigureAwait(false);
    }

    public async Task<UserNotification?> GetAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        return await db.UserNotifications
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken).ConfigureAwait(false);
    }

    public async Task<UserNotification?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await db.UserNotifications
               .FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task AddAsync(UserNotification entity, CancellationToken cancellationToken = default)
    {
        await db.UserNotifications.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(UserNotification entity, CancellationToken cancellationToken = default)
    {
        db.UserNotifications.Update(entity);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(UserNotification entity, CancellationToken cancellationToken = default)
    {
        db.UserNotifications.Remove(entity);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
