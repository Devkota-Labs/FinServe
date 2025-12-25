using Notification.Domain.Entities;
using Shared.Application.Interfaces.Repositories;

namespace Notification.Application.Interfaces;

public interface IUserNotificationRepository : IRepository<UserNotification>
{
    Task<IReadOnlyList<UserNotification>> GetByUserAsync(int userId, int take, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(int userId, CancellationToken cancellationToken = default);
    Task<UserNotification?> GetAsync(int id, int userId, CancellationToken cancellationToken = default);
}
