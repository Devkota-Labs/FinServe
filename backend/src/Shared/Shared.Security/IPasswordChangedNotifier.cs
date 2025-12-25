using Shared.Security.Contracts;

namespace Shared.Security;

public interface IPasswordChangedNotifier
{
    Task NotifyAsync(IPasswordChangedEvent passwordChangedEvent, CancellationToken cancellationToken = default);
}
