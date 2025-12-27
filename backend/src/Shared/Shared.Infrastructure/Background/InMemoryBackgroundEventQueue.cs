using Serilog;
using Shared.Common.Services;
using System.Threading.Channels;

namespace Shared.Infrastructure.Background;

internal sealed class InMemoryBackgroundEventQueue : BaseService, IBackgroundEventQ
{
    private readonly Channel<object> _channel;

    public InMemoryBackgroundEventQueue(ILogger logger, int capacity = 1000)
        : base(logger.ForContext<InMemoryBackgroundEventQueue>(), null)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false
        };

        _channel = Channel.CreateBounded<object>(options);
    }

    public async Task EnqueueAsync<T>(T @event, CancellationToken cancellationToken = default)
    {
        if (@event is null)
            throw new ArgumentNullException(nameof(@event));

        await _channel.Writer.WriteAsync(@event!, cancellationToken).ConfigureAwait(false);
    }

    public async Task<T> DequeueAsync<T>(CancellationToken cancellationToken = default)
    {
        var item = await _channel.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);

        if (item is not T typedItem)
            throw new InvalidOperationException(
                $"Expected event of type {typeof(T).Name} but received {item.GetType().Name}");

        return typedItem;
    }
}