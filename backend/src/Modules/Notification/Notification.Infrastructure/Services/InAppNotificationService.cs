using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Serilog;
using Shared.Common.Services;
using Shared.Domain.Enums.Notifications;

namespace Notification.Infrastructure.Services;

internal sealed class InAppNotificationService(ILogger logger
    , IUserNotificationRepository _repository
    )
    : BaseService(logger.ForContext<InAppNotificationService>(), null),
    IInAppNotificationService
{
    public async Task NotifyAsync(int userId, string title, string message, NotificationCategory category, NotificationSeverity severity, NotificationActionType actionType, NotificationReferenceType referenceType = NotificationReferenceType.None, int? referenceId = null, CancellationToken cancellationToken = default)
    {
        var notification = new UserNotification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Category = category,
            Severity = severity,
            ActionType = actionType,
            ReferenceType = referenceType,
            ReferenceId = referenceId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(notification, cancellationToken).ConfigureAwait(false);

        //ToDo to be implemented
        //Real-time push (best-effort)
        //try
        //{
        //    await _publisher.PublishAsync(
        //        userId,
        //        MapToDto(notification));
        //}
        //catch
        //{
        //    // Never break business flow due to SignalR failure
        //}
    }

    //private static NotificationDto MapToDto(UserNotification notification)
    //{
    //    return new NotificationDto(notification.Id, notification.Title, notification.Message,
    //        notification.Category.ToString(), notification.Severity.ToString(),
    //        notification.ActionType.ToString(), notification.ReferenceId,
    //        notification.IsRead, notification.CreatedAt);
    //}
}
