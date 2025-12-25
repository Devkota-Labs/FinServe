using Shared.Security.Contracts;

namespace Shared.Security;

public interface IUserUnlockedNotifier
{
    Task NotifyAsync(IUserUnlockedEvent userUnlockedEvent, CancellationToken cancellationToken = default);
}
