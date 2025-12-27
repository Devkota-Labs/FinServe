using Auth.Domain.Entities;
using Shared.Application.Interfaces.Repositories;

namespace Auth.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<ICollection<RefreshToken>?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task RevokeAllAsync(int userId, CancellationToken cancellationToken = default);
}
