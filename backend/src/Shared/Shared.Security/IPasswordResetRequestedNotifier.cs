using Shared.Security.Contracts;

namespace Shared.Security;

public interface IPasswordResetRequestedNotifier
{
    Task NotifyAsync(IPasswordResetRequestedEvent passwordResetRequestEvent, CancellationToken cancellationToken = default);
}
