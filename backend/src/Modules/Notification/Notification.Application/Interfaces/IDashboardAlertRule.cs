using Shared.Security.Contracts;

namespace Notification.Application.Interfaces;

public interface IDashboardAlertRule
{
    Task<IReadOnlyList<NotifyCommand>> EvaluateAsync(ILoginEvent loginEvent, CancellationToken cancellationToken = default);
}
