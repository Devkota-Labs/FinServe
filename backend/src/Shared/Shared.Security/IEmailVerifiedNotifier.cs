using Shared.Security.Contracts;

namespace Shared.Security;

public interface IEmailVerifiedNotifier
{
    Task NotifyAsync(IEmailVerifiedEvent emailVerifiedEvent, CancellationToken cancellationToken = default);
}