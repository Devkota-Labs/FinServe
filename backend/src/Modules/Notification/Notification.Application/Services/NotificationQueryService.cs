using Notification.Application.Dtos;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;

namespace Notification.Application.Services;

internal sealed class NotificationQueryService(ILogger logger, IInAppNotificationRepository repo)
    : BaseService(logger.ForContext<NotificationQueryService>(), null)
    , INotificationQueryService
{
    public async Task<Result<InAppNotificationDto>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repo.GetAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<InAppNotificationDto>("Notification not found");

        return Result.Ok(Map(entity));
    }

    public async Task<Result<ICollection<InAppNotificationDto>>> GetByUserIdAsync(int userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetByUserIdAsync(userId, page, pageSize, cancellationToken).ConfigureAwait(false);

        var result = entities.Select(Map).ToList();

        return Result.Ok<ICollection<InAppNotificationDto>>(result);
    }

    public async Task<Result<int>> GetUnreadCountAsync(int userId, CancellationToken cancellationToken = default)
    {
        var unreadCount = await repo.GetUnreadCountAsync(userId, cancellationToken).ConfigureAwait(false);

        return Result.Ok(unreadCount);
    }

    public async Task<Result> MarkAsReadAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repo.GetAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<InAppNotificationDto>("Notification not found");

        entity.IsRead = true;

        await repo.MarkAsReadAsync(id, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Notification maked as read successfully", Map(entity));
    }

    private static InAppNotificationDto Map(InAppNotification notification)
    {
        return new InAppNotificationDto
        {
            Id = notification.Id,
            Type = notification.Type,
            Category = notification.Category,
            Severity = notification.Severity,
            ActionType = notification.ActionType,
            ReferenceType = notification.ReferenceType,
            ReferenceId = notification.ReferenceId,
            Title = notification.Title,
            Body = notification.Body,
            ActionUrl = notification.ActionUrl,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }
}