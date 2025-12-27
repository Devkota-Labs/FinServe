namespace Shared.Infrastructure.Background;

public interface IBackgroundEventQ
{
    Task EnqueueAsync<T>(T @event, CancellationToken cancellationToken = default);
    Task<T> DequeueAsync<T>(CancellationToken cancellationToken = default);
}
