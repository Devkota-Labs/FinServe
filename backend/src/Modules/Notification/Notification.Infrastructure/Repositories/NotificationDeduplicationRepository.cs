using Microsoft.EntityFrameworkCore;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Notification.Infrastructure.Db;

namespace Notification.Infrastructure.Repositories;

internal sealed class NotificationDeduplicationRepository(NotificationDbContext db)
    : INotificationDeduplicationRepository
{
    public async Task<bool> ExistsAsync(int userId, NotificationType type, NotificationChannelType channel, string? dedupKey, DateTime since, CancellationToken cancellationToken = default)
    {
        return await db.Deduplications
            .AnyAsync(x =>
                x.UserId == userId &&
                x.Type == type &&
                x.Channel == channel &&
                x.DedupKey == dedupKey &&
                x.CreatedAt >= since,
                cancellationToken).ConfigureAwait(false);
    }

    public async Task AddAsync(NotificationDeduplication entity, CancellationToken cancellationToken = default)
    {
        db.Add(entity);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
