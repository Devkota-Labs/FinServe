using Notification.Application.Models;
using Notification.Domain.Enums;
using Notification.Domain.Events;

namespace Notification.Application.Interfaces;

public interface INotificationTemplateRenderer
{
    NotificationMessage Render(NotificationEvent notification, NotificationChannelType channel);
}
