using Shared.Domain.Enums.Notifications;

namespace Notification.Application.Interfaces;

public interface IInAppNotificationService
{
    Task NotifyAsync(
        int userId,
        string title,
        string message,
        NotificationCategory category,
        NotificationSeverity severity,
        NotificationActionType actionType,
        NotificationReferenceType referenceType = NotificationReferenceType.None,
        int? referenceId = null, CancellationToken cancellationToken = default);
}