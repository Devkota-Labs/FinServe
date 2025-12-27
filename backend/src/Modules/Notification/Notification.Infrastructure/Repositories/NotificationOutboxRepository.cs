using Microsoft.EntityFrameworkCore;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Notification.Infrastructure.Db;

namespace Notification.Infrastructure.Repositories;

internal sealed class NotificationOutboxRepository(NotificationDbContext _db)
    : INotificationOutboxRepository
{
    public async Task AddAsync(NotificationOutbox outbox, CancellationToken cancellationToken = default)
    {
        _db.Outbox.Add(outbox);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(NotificationOutbox outbox, CancellationToken cancellationToken = default)
    {
        _db.Outbox.Update(outbox);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<NotificationOutbox>> GetFailedAsync(int maxRetries, CancellationToken cancellationToken = default)
    {
        return await _db.Outbox
            .Where(x => x.Status == NotificationDeliveryStatus.Failed &&
                        x.RetryCount < maxRetries)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }
}