using Notification.Application.Handlers;
using Serilog;
using Shared.Common.Services;
using Shared.Security;
using Shared.Security.Contracts;

namespace Notification.Application.Adapters;

internal sealed class EmailChangedNotifier(
    ILogger logger,
    EmailChangedNotificationHandler handler
    )
    : BaseService(logger.ForContext<EmailChangedNotifier>(), null)
    , IEmailChangedNotifier
{
    public async Task NotifyAsync(IEmailChangedEvent emailChangedEvent, CancellationToken cancellationToken = default)
    {
        await handler.HandleAsync(emailChangedEvent, cancellationToken).ConfigureAwait(false);
    }
}