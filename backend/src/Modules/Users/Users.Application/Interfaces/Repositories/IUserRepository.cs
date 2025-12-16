using Shared.Application.Interfaces.Repositories;
using Users.Domain.Entities;

namespace Users.Application.Interfaces.Repositories;

public interface IUserRepository : IMasterRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByMobileAsync(string mobile, CancellationToken cancellationToken = default);
    Task<User?> GetByUserNameOrEmailAsync(string userNameOrEmail, CancellationToken cancellationToken = default);
}
