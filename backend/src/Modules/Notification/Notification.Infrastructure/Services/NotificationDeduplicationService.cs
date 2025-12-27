using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Serilog;
using Shared.Common.Services;
using Shared.Common.Utils;

namespace Notification.Infrastructure.Services;

internal sealed class NotificationDeduplicationService(
    ILogger logger,
    INotificationDeduplicationRepository _repository
    )
    : BaseService(logger.ForContext<NotificationDeduplicationService>(), null)
    , INotificationDeduplicationService
{
    public async Task<bool> IsDuplicateAsync(int userId, NotificationType type, NotificationChannelType channel, string? dedupKey, TimeSpan window, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(
            userId,
            type,
            channel,
            dedupKey,
            DateTimeUtil.Now.Subtract(window),
            cancellationToken).ConfigureAwait(false);

        if (exists)
            return true;

        await _repository.AddAsync(new NotificationDeduplication
        {
            UserId = userId,
            Type = type,
            Channel = channel,
            DedupKey = dedupKey
        }, cancellationToken).ConfigureAwait(false);

        return false;
    }
}
