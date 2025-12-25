using Shared.Security.Contracts;

namespace Shared.Security;

public interface IPasswordResetSuccessNotifier
{
    Task NotifyAsync(IPasswordResetSuccessEvent passwordResetSuccessEvent, CancellationToken cancellationToken = default);
}
