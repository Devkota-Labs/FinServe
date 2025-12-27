using Notification.Domain.Enums;
using Notification.Domain.Metadata;

namespace Notification.Domain.Events;

public sealed class NotificationEvent(NotificationType type, int userId, Dictionary<string, object?> data)
{
    public NotificationType Type { get; } = type;
    public int UserId { get; } = userId;
    public Dictionary<string, object?> Data { get; } = data;
    public string? DeduplicationKey { get; }
    public string? ToEmail { get; }

    public NotificationEvent(NotificationType type, int userId, string? deduplicationKey, string? toEmail, Dictionary<string, object?> data)
        : this(type, userId, data)
    {
        DeduplicationKey = deduplicationKey;
        ToEmail = toEmail;
    }

    public NotificationDefinition Definition =>
        NotificationDefinitionRegistry.Get(Type);
}