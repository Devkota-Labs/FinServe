using Notification.Domain.Events;

namespace Notification.Application.Interfaces;

public interface INotificationOrchestrator
{
    Task SendAsync(NotificationEvent notification, CancellationToken cancellationToken = default);
}
