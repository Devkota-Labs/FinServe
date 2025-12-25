using Notification.Application.Handlers;
using Serilog;
using Shared.Common.Services;
using Shared.Security;
using Shared.Security.Contracts;

namespace Notification.Application.Adapters;

internal sealed class EmailVerifiedNotifier(
    ILogger logger,
    EmailVerifiedNotificationHandler handler
    )
    : BaseService(logger.ForContext<EmailVerifiedNotifier>(), null)
    , IEmailVerifiedNotifier
{
    public async Task NotifyAsync(IEmailVerifiedEvent emailVerifiedEvent, CancellationToken cancellationToken = default)
    {
        await handler.HandleAsync(emailVerifiedEvent, cancellationToken).ConfigureAwait(false);
    }
}