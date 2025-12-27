using Notification.Domain.Enums;

namespace Notification.Application.Models;

public sealed class NotificationMessage
{
    public int UserId { get; init; }
    public NotificationType Type { get; init; }
    public NotificationCategory Category { get; init; }
    public NotificationSeverity Severity { get; init; }
    public NotificationActionType ActionType { get; init; }
    public NotificationReferenceType ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public NotificationChannelType Channel { get; init; }

    public string Title { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public Uri? ActionUrl { get; init; }

    public string? To { get; init; }
    public string? Phone { get; init; }
}
