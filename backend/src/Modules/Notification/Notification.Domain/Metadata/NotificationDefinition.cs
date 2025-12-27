using Notification.Domain.Enums;

namespace Notification.Domain.Metadata;

public sealed class NotificationDefinition
{
    public NotificationType Type { get; init; }
    public NotificationCategory Category { get; init; }
    public NotificationSeverity Severity { get; init; }
    public NotificationActionType ActionType { get; init; }
    public IReadOnlyCollection<NotificationChannelType> Channels { get; init; } = [];
}
