using Notification.Application.Services;
using Serilog;
using Shared.Common.Services;
using Shared.Security;
using Shared.Security.Contracts;

namespace Notification.Application.Adapters;

internal sealed class LoginAlertNotifier(
    ILogger logger,
    LoginAlertService loginAlertService
    )
    : BaseService(logger.ForContext<LoginAlertNotifier>(), null)
    , ILoginAlertNotifier
{
    public async Task NotifyAsync(ILoginEvent loginEvent, CancellationToken cancellationToken)
    {
        await loginAlertService.HandleAsync(loginEvent, cancellationToken).ConfigureAwait(false);
    }
}
