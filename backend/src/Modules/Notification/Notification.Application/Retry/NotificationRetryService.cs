using Microsoft.Extensions.Options;
using Notification.Application.Interfaces;
using Notification.Application.Models;
using Notification.Application.Options;
using Notification.Domain.Enums;
using Serilog;
using Shared.Common.Services;
using Shared.Common.Utils;
using System.Text.Json;

namespace Notification.Application.Retry;

internal sealed class NotificationRetryService(
    ILogger logger,
    INotificationOutboxRepository _outboxRepository,
    IEnumerable<INotificationChannel> channels,
    IOptions<ScheduledJobsOptions> scheduledJobOptions
    )
    : BaseService(logger.ForContext<NotificationRetryService>(), null)
    , INotificationRetryService
{
    public async Task RetryAsync(CancellationToken cancellationToken = default)
    {
        var failedNotifications =
            await _outboxRepository.GetFailedAsync(scheduledJobOptions.Value.NotificationMaxRetryCount, cancellationToken).ConfigureAwait(false);

        foreach (var outbox in failedNotifications)
        {
            NotificationMessage message;

#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                message = JsonSerializer.Deserialize<NotificationMessage>(outbox.Payload)!;
            }
            catch
            {
                // Payload corrupted – mark as failed permanently
                outbox.RetryCount = scheduledJobOptions.Value.NotificationMaxRetryCount;
                await _outboxRepository.UpdateAsync(outbox, cancellationToken).ConfigureAwait(false);
                continue;
            }
#pragma warning restore CA1031 // Do not catch general exception types

            var channel = channels.SingleOrDefault(c => c.ChannelType == outbox.Channel);

            if (channel is null)
                continue;

#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                await channel.SendAsync(message, cancellationToken).ConfigureAwait(false);

                outbox.Status = NotificationDeliveryStatus.Sent;
                outbox.SentAt = DateTimeUtil.Now;
            }
            catch (Exception ex)
            {
                outbox.RetryCount++;
                outbox.Error = ex.Message;
                outbox.Status = NotificationDeliveryStatus.Failed;
            }
#pragma warning restore CA1031 // Do not catch general exception types

            await _outboxRepository.UpdateAsync(outbox, cancellationToken).ConfigureAwait(false);
        }
    }
}
