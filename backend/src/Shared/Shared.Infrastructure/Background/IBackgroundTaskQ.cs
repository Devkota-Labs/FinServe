namespace Shared.Infrastructure.Background;

public interface IBackgroundTaskQ
{
    void Queue(Func<IServiceProvider, Task> workItem);
    Task<Func<IServiceProvider, Task>> DequeueAsync(CancellationToken cancellationToken);
}
