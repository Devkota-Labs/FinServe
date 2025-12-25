using Shared.Security.Contracts;

namespace Shared.Security;

public interface ILoginAlertNotifier
{
    Task NotifyAsync(ILoginEvent loginEvent, CancellationToken cancellationToken = default);
}
