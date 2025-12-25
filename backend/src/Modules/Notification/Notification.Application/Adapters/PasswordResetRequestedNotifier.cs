using Notification.Application.Handlers;
using Serilog;
using Shared.Common.Services;
using Shared.Security;
using Shared.Security.Contracts;

namespace Notification.Application.Adapters;

internal sealed class PasswordResetRequestedNotifier(
    ILogger logger,
    PasswordResetRequestedNotificationHandler handler
    )
    : BaseService(logger.ForContext<PasswordResetRequestedNotifier>(), null)
    , IPasswordResetRequestedNotifier
{
    public async Task NotifyAsync(IPasswordResetRequestedEvent passwordResetRequestedEvent, CancellationToken cancellationToken = default)
    {
        await handler.HandleAsync(passwordResetRequestedEvent, cancellationToken).ConfigureAwait(false);
    }
}

