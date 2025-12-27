using Notification.Domain.Entities;

namespace Notification.Application.Interfaces;

public interface INotificationOutboxRepository
{
    Task AddAsync(NotificationOutbox outbox, CancellationToken cancellationToken = default);
    Task UpdateAsync(NotificationOutbox outbox, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NotificationOutbox>> GetFailedAsync(int maxRetries, CancellationToken cancellationToken = default);
}
