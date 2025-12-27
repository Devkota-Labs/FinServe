using Notification.Domain.Enums;
using Notification.Domain.Events;

namespace Notification.Application.Interfaces;

public interface INotificationDispatcher
{
    Task DispatchAsync(NotificationEvent notification, NotificationChannelType channel, CancellationToken cancellationToken = default);
}
