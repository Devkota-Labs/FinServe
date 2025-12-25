using Notification.Application.Handlers;
using Serilog;
using Shared.Common.Services;
using Shared.Security;
using Shared.Security.Contracts;

namespace Notification.Application.Adapters;

internal sealed class PasswordChangedNotifier(
    ILogger logger,
    PasswordChangedNotificationHandler handler
    )
    : BaseService(logger.ForContext<PasswordChangedNotifier>(), null)
    , IPasswordChangedNotifier
{
    public async Task NotifyAsync(IPasswordChangedEvent passwordChangedEvent, CancellationToken cancellationToken = default)
    {
        await handler.HandleAsync(passwordChangedEvent, cancellationToken).ConfigureAwait(false);
    }
}

