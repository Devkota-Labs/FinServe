using Notification.Domain.Deduplication;
using Notification.Domain.Enums;

namespace Notification.Application.Interfaces;

public interface INotificationDedupRuleResolver
{
    NotificationDedupRule Resolve(NotificationType type);
}
