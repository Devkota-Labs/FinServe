using Notification.Domain.Enums;

namespace Notification.Application.Interfaces;

public interface INotificationDeduplicationService
{
    Task<bool> IsDuplicateAsync(int userId, NotificationType type, NotificationChannelType channel, string? dedupKey, TimeSpan window, CancellationToken cancellationToken = default);
}