using Notification.Domain.Entities;
using Notification.Domain.Enums;

namespace Notification.Application.Interfaces;

public interface INotificationDeduplicationRepository
{
    Task<bool> ExistsAsync(int userId, NotificationType type, NotificationChannelType channel, string? dedupKey, DateTime since, CancellationToken cancellationToken = default);
    Task AddAsync(NotificationDeduplication entity, CancellationToken cancellationToken = default);
}
