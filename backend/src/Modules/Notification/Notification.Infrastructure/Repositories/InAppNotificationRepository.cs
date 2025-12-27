using Microsoft.EntityFrameworkCore;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Infrastructure.Db;
using Shared.Common.Utils;

namespace Notification.Infrastructure.Repositories;

internal sealed class InAppNotificationRepository(NotificationDbContext db)
    : IInAppNotificationRepository
{
    public async Task AddAsync(InAppNotification notification, CancellationToken cancellationToken = default)
    {
        db.InAppNotifications.Add(notification);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<InAppNotification> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<InAppNotification>> GetByUserIdAsync(int userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await db.InAppNotifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> GetUnreadCountAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await db.InAppNotifications
          .CountAsync(x => x.UserId == userId && !x.IsRead, cancellationToken).ConfigureAwait(false);
    }

    public async Task MarkAsReadAsync(int notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await db.InAppNotifications
            .FirstOrDefaultAsync(
                x => x.Id == notificationId,
                cancellationToken).ConfigureAwait(false);

        if (notification is null || notification.IsRead)
            return;

        notification.IsRead = true;
        notification.ReadAt = DateTimeUtil.Now;

        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
