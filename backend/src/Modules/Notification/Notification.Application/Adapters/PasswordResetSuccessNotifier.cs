using Notification.Application.Handlers;
using Serilog;
using Shared.Common.Services;
using Shared.Security;
using Shared.Security.Contracts;

namespace Notification.Application.Adapters;

internal sealed class PasswordResetSuccessNotifier(
    ILogger logger,
    PasswordResetSuccessNotificationHandler handler
    )
    : BaseService(logger.ForContext<PasswordResetSuccessNotifier>(), null)
    , IPasswordResetSuccessNotifier
{
    public async Task NotifyAsync(IPasswordResetSuccessEvent passwordResetSuccessEvent, CancellationToken cancellationToken = default)
    {
        await handler.HandleAsync(passwordResetSuccessEvent, cancellationToken).ConfigureAwait(false);
    }
}

