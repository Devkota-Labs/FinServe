using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Interfaces;
using Notification.Domain.Events;
using Serilog;
using Shared.Common.Services;
using Shared.Infrastructure.Background;

namespace Notification.Infrastructure.HostedServices;

internal sealed class NotificationEventConsumer(
    ILogger logger,
    IServiceScopeFactory scopeFactory,
    IBackgroundEventQ queue
    )
    : BaseBackgroundService(logger.ForContext<NotificationEventConsumer>(), null)
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Logger.Information("{Name} started.", Name);

        while (!cancellationToken.IsCancellationRequested)
        {
            NotificationEvent notificationEvent;

#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                //Wait for next event
                notificationEvent = await queue.DequeueAsync<NotificationEvent>(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // graceful shutdown
                break;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error while dequeuing NotificationEvent.");
                continue;
            }
#pragma warning restore CA1031 // Do not catch general exception types

#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                //Create DI scope
                using var scope = scopeFactory.CreateScope();

                var orchestrator = scope.ServiceProvider.GetRequiredService<INotificationOrchestrator>();

                // 3️ Process notification
                await orchestrator.SendAsync(notificationEvent, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // shutdown requested
                break;
            }
            catch (Exception ex)
            {
                // IMPORTANT:
                // Do NOT crash the service.
                // The outbox + retry service will handle failures.
                Logger.Error(
                    ex,
                    "Error processing NotificationEvent. Type: {Type}, UserId: {UserId}",
                    notificationEvent.Type,
                    notificationEvent.UserId);
            }
#pragma warning restore CA1031 // Do not catch general exception types

        }

        Logger.Information("{Name} stopping.", Name);
    }
}
