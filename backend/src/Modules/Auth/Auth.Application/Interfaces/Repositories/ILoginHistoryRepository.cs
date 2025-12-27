using Auth.Domain.Entities;
using Shared.Application.Interfaces.Repositories;

namespace Auth.Application.Interfaces.Repositories;

public interface ILoginHistoryRepository : IRepository<LoginHistory>
{
    Task<LoginHistory?> GetBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int userId, string ip, string userAgent, CancellationToken cancellationToken = default);
}