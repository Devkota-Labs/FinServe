using Shared.Domain.Notifications;

namespace Notification.Application.Interfaces;

public interface INotificationDeduplicationService
{
    Task<bool> ExistsAsync(int userId, NotificationTemplateKey key, TimeSpan window, CancellationToken cancellationToken = default);
}