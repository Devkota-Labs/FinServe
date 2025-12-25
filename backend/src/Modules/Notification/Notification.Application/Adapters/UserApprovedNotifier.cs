using Notification.Application.Handlers;
using Serilog;
using Shared.Common.Services;
using Shared.Security;
using Shared.Security.Contracts;

namespace Notification.Application.Adapters;

internal sealed class UserApprovedNotifier(
    ILogger logger,
    UserApprovedNotificationHandler handler
    )
    : BaseService(logger.ForContext<UserApprovedNotifier>(), null)
    , IUserApprovedNotifier
{
    public async Task NotifyAsync(IUserApprovedEvent userApprovedEvent, CancellationToken cancellationToken = default)
    {
        await handler.HandleAsync(userApprovedEvent, cancellationToken).ConfigureAwait(false);
    }
}