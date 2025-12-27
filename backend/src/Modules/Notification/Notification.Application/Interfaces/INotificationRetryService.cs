namespace Notification.Application.Interfaces;

public interface INotificationRetryService
{
    Task RetryAsync(CancellationToken cancellationToken = default);
}
