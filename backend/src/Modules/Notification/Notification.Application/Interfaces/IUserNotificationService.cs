using Notification.Application.Dtos;
using Shared.Application.Results;

namespace Notification.Application.Interfaces;

public interface IUserNotificationService
{
    public Task<Result<NotificationDto>> GetAsync(int id, CancellationToken cancellationToken = default);
    public Task<Result<ICollection<NotificationDto>>> GetByUserIdAsync(int userId, int take, CancellationToken cancellationToken = default);
    Task<Result<int>> GetUnreadCountAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result> MarkAsReadAsync(int id, CancellationToken cancellationToken = default);
}
