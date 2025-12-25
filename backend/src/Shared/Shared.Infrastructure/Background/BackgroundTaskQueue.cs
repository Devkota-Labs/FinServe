using Serilog;
using Shared.Common.Services;
using System.Threading.Channels;

namespace Shared.Infrastructure.Background;

internal sealed class BackgroundTaskQueue(ILogger logger)
    : BaseService(logger.ForContext<BackgroundTaskQueue>(), null)
    , IBackgroundTaskQ
{
    private readonly Channel<Func<IServiceProvider, Task>> _queue =
        Channel.CreateUnbounded<Func<IServiceProvider, Task>>();

    public void Queue(Func<IServiceProvider, Task> workItem)
    {
        _queue.Writer.TryWrite(workItem);
    }

    public async Task<Func<IServiceProvider, Task>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
    }
}