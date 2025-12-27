using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Notification.Domain.Events;
using Serilog;
using Shared.Common.Services;
using Shared.Common.Utils;
using System.Text.Json;

namespace Notification.Application.Dispatchers;

internal sealed class NotificationDispatcher(
    ILogger logger,
    IEnumerable<INotificationChannel> channels,
    INotificationTemplateRenderer renderer,
    INotificationOutboxRepository outboxRepo,
    INotificationDedupRuleResolver ruleResolver,
    INotificationDeduplicationService deduplicationService
    )
    : BaseService(logger.ForContext<NotificationDispatcher>(), null),
    INotificationDispatcher
{
    public async Task DispatchAsync(NotificationEvent notification, NotificationChannelType channel, CancellationToken cancellationToken = default)
    {
        var rule = ruleResolver.Resolve(notification.Type);
        if (rule.Enabled)
        {
            var dedupKey = rule.DedupKeyResolver(notification.Data);

            var isDuplicate = await deduplicationService.IsDuplicateAsync(
                userId: notification.UserId,
                type: notification.Type,
                channel: channel,
                dedupKey: dedupKey,
                window: rule.Window,
                cancellationToken).ConfigureAwait(false);

            if (isDuplicate)
                return; //SKIP
        }

        var message = renderer.Render(notification, channel);

        var outbox = new NotificationOutbox
        {
            UserId = message.UserId,
            Type = message.Type,
            Category = message.Category,
            Severity = message.Severity,
            Channel = channel,
            ReferenceType = message.ReferenceType,
            ReferenceId = message.ReferenceId,
            Payload = JsonSerializer.Serialize(message),
            Status = NotificationDeliveryStatus.Pending
        };

        await outboxRepo.AddAsync(outbox, cancellationToken).ConfigureAwait(false);

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            await channels.Single(c => c.ChannelType == channel)
                .SendAsync(message, cancellationToken).ConfigureAwait(false);

            outbox.Status = NotificationDeliveryStatus.Sent;
            outbox.SentAt = DateTimeUtil.Now;
        }
        catch (Exception ex)
        {
            outbox.Status = NotificationDeliveryStatus.Failed;
            outbox.Error = ex.Message;
            outbox.RetryCount++;
        }
#pragma warning restore CA1031 // Do not catch general exception types

        await outboxRepo.UpdateAsync(outbox, cancellationToken).ConfigureAwait(false);
    }
}
