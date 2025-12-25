using Shared.Security.Contracts;

namespace Shared.Security;

public interface IUserApprovedNotifier
{
    Task NotifyAsync(IUserApprovedEvent userApprovedEvent, CancellationToken cancellationToken = default);
}
