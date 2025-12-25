using Shared.Domain.Enums.Notifications;
using Shared.Domain.Notifications;

namespace Notification.Application;

public sealed class NotifyCommand
{
    public int UserId { get; set; }

    public NotificationTemplateKey TemplateKey { get; set; }
    public object Model { get; set; } = default!;

    public NotificationCategory Category { get; set; }
    public NotificationSeverity Severity { get; set; }
    public NotificationActionType ActionType { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
    public required ICollection<NotificationChannel> Channels { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
}