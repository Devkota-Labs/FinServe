using Notification.Application.Interfaces;
using Notification.Domain.Deduplication;
using Notification.Domain.Enums;
using Serilog;
using Shared.Common.Services;

namespace Notification.Application.Services;

internal sealed class NotificationDedupRuleResolver(ILogger logger)
    : BaseService(logger.ForContext<NotificationDedupRuleResolver>(), null)
    , INotificationDedupRuleResolver
{
    public NotificationDedupRule Resolve(NotificationType type)
        => NotificationDedupRuleRegistry.Get(type);
}
