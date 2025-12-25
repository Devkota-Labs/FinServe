using Shared.Domain.Entities;
using Shared.Domain.Enums.Notifications;
using Shared.Domain.Notifications;

namespace Notification.Domain.Entities;

public sealed class UserNotification : BaseEntity
{
    public int UserId { get; set; }

    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;

    public NotificationTemplateKey TemplateKey { get; set; }

    public NotificationCategory Category { get; set; }
    public NotificationSeverity Severity { get; set; }

    public NotificationActionType ActionType { get; set; }
    public NotificationReferenceType ReferenceType { get; set; }
    public int? ReferenceId { get; set; }

    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
