using Notification.Domain.Enums;
using Shared.Common.Utils;
using Shared.Domain.Entities;

namespace Notification.Domain.Entities;

public class NotificationDeduplication : BaseEntity
{
    public int UserId { get; set; }
    public NotificationType Type { get; set; }
    public NotificationChannelType Channel { get; set; }

    // optional key (deviceId, ip, etc.)
    public string? DedupKey { get; set; }
    public DateTime CreatedAt { get; set; } = DateTimeUtil.Now;
}