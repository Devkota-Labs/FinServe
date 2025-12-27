using Notification.Application.Interfaces;
using Notification.Domain.Events;
using Serilog;
using Shared.Common.Services;

namespace Notification.Application.Orchestrator;

internal sealed class NotificationOrchestrator(
ILogger logger,
INotificationChannelResolver _resolver,
INotificationDispatcher _dispatcher
)
: BaseService(logger.ForContext<NotificationOrchestrator>(), null)
, INotificationOrchestrator
{
    public async Task SendAsync(NotificationEvent notification, CancellationToken cancellationToken = default)
    {
        foreach (var channel in await _resolver.ResolveAsync(notification, cancellationToken).ConfigureAwait(false))
            await _dispatcher.DispatchAsync(notification, channel, cancellationToken).ConfigureAwait(false);
    }
}