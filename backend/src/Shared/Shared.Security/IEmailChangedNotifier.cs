using Shared.Security.Contracts;

namespace Shared.Security;

public interface IEmailChangedNotifier
{
    Task NotifyAsync(IEmailChangedEvent emailChangedEvent, CancellationToken cancellationToken = default);
}
