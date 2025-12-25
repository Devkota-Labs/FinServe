using Notification.Application.Dtos;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;

namespace Notification.Application.Services;

internal sealed class UserNotificationService(ILogger logger, IUserNotificationRepository repo)
    : BaseService(logger.ForContext<UserNotificationService>(), null)
    , IUserNotificationService
{
    public async Task<Result<NotificationDto>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<NotificationDto>("Notification not found");

        return Result.Ok(Map(entity));
    }

    public async Task<Result<ICollection<NotificationDto>>> GetByUserIdAsync(int userId, int take, CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetByUserAsync(userId, take, cancellationToken).ConfigureAwait(false);

        var result = entities.Select(Map).ToList();

        return Result.Ok<ICollection<NotificationDto>>(result);
    }

    public async Task<Result<int>> GetUnreadCountAsync(int userId, CancellationToken cancellationToken = default)
    {
        var unreadCount = await repo.GetUnreadCountAsync(userId, cancellationToken).ConfigureAwait(false);

        return Result.Ok(unreadCount);
    }

    public async Task<Result> MarkAsReadAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<NotificationDto>("Notification not found");

        entity.IsRead = true;

        await repo.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Notification maked as read successfully", Map(entity));
    }

    private static NotificationDto Map(UserNotification notification)
    {
        return new(notification.Id, notification.Title, notification.Message, notification.Category.ToString(), notification.Severity.ToString(), notification.ActionType.ToString(), notification.ReferenceId, notification.IsRead, notification.CreatedAt);
    }
}