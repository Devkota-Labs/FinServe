using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Repositories;

public interface IPasswordHistoryRepository
{
    Task<List<PasswordHistory>> GetRecentAsync(int userId, int limit, CancellationToken cancellationToken);
    Task AddAsync(PasswordHistory history, CancellationToken cancellationToken);
}
