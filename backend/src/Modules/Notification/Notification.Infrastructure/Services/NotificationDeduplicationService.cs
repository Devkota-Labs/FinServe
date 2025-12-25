using Microsoft.EntityFrameworkCore;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Db;
using Serilog;
using Shared.Common.Services;
using Shared.Domain.Notifications;

namespace Notification.Infrastructure.Services;

internal sealed class NotificationDeduplicationService(ILogger logger, NotificationDbContext notificationDbContext)
: BaseService(logger.ForContext<NotificationDbContext>(), null)
    , INotificationDeduplicationService
{
    public async Task<bool> ExistsAsync(int userId, NotificationTemplateKey key, TimeSpan window, CancellationToken cancellationToken = default)
    {
        var since = DateTime.UtcNow.Subtract(window);

        return await notificationDbContext.UserNotifications.AnyAsync(x =>
            x.UserId == userId &&
            x.TemplateKey == key &&
            x.CreatedAt >= since, cancellationToken).ConfigureAwait(false);
    }
}