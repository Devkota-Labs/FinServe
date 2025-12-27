using Notification.Application.Interfaces;
using Notification.Application.Orchestrator;
using Notification.Domain.Enums;
using Notification.Domain.Events;
using Serilog;
using Shared.Common.Services;

namespace Notification.Application.Resolvers;

internal sealed class DefaultNotificationChannelResolver(ILogger logger)
: BaseService(logger.ForContext<NotificationOrchestrator>(), null),
    INotificationChannelResolver
{
    public Task<IReadOnlyCollection<NotificationChannelType>> ResolveAsync(NotificationEvent notification, CancellationToken cancellationToken = default)
        => Task.FromResult(notification.Definition.Channels);
}
