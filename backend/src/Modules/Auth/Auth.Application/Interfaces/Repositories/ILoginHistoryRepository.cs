using Auth.Domain.Entities;
using Shared.Application.Interfaces;

namespace Auth.Application.Interfaces.Repositories;

public interface ILoginHistoryRepository : IRepository<LoginHistory>
{
    Task<LoginHistory?> GetBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default);
}