using Notification.Domain.Enums;
using Notification.Domain.Events;

namespace Notification.Application.Interfaces;

public interface INotificationChannelResolver
{
    Task<IReadOnlyCollection<NotificationChannelType>> ResolveAsync(NotificationEvent notification, CancellationToken cancellationToken = default);
}
