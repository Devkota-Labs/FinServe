using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Notification.Application.Interfaces;
using Notification.Application.Options;
using Serilog;
using Shared.Common.Services;

namespace Notification.Infrastructure.HostedServices;

internal sealed class NotificationRetryHostedService(
    ILogger logger,
    IServiceScopeFactory scopeFactory,
    IOptions<ScheduledJobsOptions> scheduledJobOptions
    )
    : BaseBackgroundService(logger.ForContext<PasswordReminderHostedService>(), null)
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Logger.Information("{Name} started.", Name);

        while (!cancellationToken.IsCancellationRequested)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                using var scope = scopeFactory.CreateScope();

                var retryService =
                    scope.ServiceProvider.GetRequiredService<INotificationRetryService>();

                await retryService.RetryAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // graceful shutdown
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred while retrying notifications.");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            try
            {
                await Task.Delay(TimeSpan.FromMinutes(scheduledJobOptions.Value.NotificationRetryIntervalMinutes), cancellationToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                // shutdown
            }
        }

        Logger.Information("{Name} stopped.", Name);
    }
}
