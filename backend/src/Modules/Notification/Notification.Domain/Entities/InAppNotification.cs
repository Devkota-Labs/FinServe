using Notification.Domain.Enums;
using Shared.Domain.Entities;

namespace Notification.Domain.Entities;

public sealed class InAppNotification : BaseEntity
{
    public int UserId { get; set; }

    public NotificationType Type { get; set; }
    public NotificationCategory Category { get; set; }
    public NotificationSeverity Severity { get; set; }
    public NotificationActionType ActionType { get; set; }
    public NotificationReferenceType ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public Uri? ActionUrl { get; set; }

    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
