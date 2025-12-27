using Notification.Domain.Enums;

namespace Notification.Application.Dtos;

public sealed class InAppNotificationDto
{
    public int Id { get; init; }

    public NotificationType Type { get; init; }
    public NotificationCategory Category { get; init; }
    public NotificationSeverity Severity { get; init; }
    public NotificationActionType ActionType { get; init; }
    public NotificationReferenceType ReferenceType { get; init; }
    public int? ReferenceId { get; init; }

    public string Title { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public Uri? ActionUrl { get; init; }

    public bool IsRead { get; init; }
    public DateTime CreatedAt { get; init; }
}