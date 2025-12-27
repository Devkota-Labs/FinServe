using Notification.Application.Models;
using Notification.Domain.Enums;

namespace Notification.Application.Interfaces;

public interface INotificationChannel
{
    NotificationChannelType ChannelType { get; }
    Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default);
}
