using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Common.Services;

namespace Shared.Infrastructure.Background;

internal sealed class QueuedHostedService(
    ILogger logger,
    IServiceProvider serviceProvider,
    IBackgroundTaskQ queue
    )
    : BaseBackgroundService(logger.ForContext<QueuedHostedService>(), null)
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var workItem = await queue.DequeueAsync(cancellationToken).ConfigureAwait(false);

            using var scope = serviceProvider.CreateScope();
            await workItem(scope.ServiceProvider).ConfigureAwait(false);
        }
    }
}