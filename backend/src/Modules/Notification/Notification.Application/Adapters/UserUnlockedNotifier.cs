using Notification.Application.Handlers;
using Serilog;
using Shared.Common.Services;
using Shared.Security;
using Shared.Security.Contracts;

namespace Notification.Application.Adapters;

internal sealed class UserUnlockedNotifier(
    ILogger logger,
    UserUnlockedNotificationHandler handler
    )
    : BaseService(logger.ForContext<UserUnlockedNotifier>(), null)
    , IUserUnlockedNotifier
{
    public async Task NotifyAsync(IUserUnlockedEvent userUnlockedEvent, CancellationToken cancellationToken = default)
    {
        await handler.HandleAsync(userUnlockedEvent, cancellationToken).ConfigureAwait(false);
    }
}

