using Notification.Domain.Enums;
using Shared.Domain.Entities;

namespace Notification.Domain.Entities;

public class NotificationOutbox : BaseEntity
{
    public int UserId { get; set; }

    public NotificationType Type { get; set; }
    public NotificationCategory Category { get; set; }
    public NotificationSeverity Severity { get; set; }
    public NotificationChannelType Channel { get; set; }
    public NotificationReferenceType ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public string Payload { get; set; } = null!;
    public NotificationDeliveryStatus Status { get; set; }

    public int RetryCount { get; set; }
    public string? Error { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
}
