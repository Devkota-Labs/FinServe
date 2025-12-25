using Shared.Application.Interfaces.Repositories;
using Users.Domain.Entities;

namespace Users.Application.Interfaces.Repositories;

public interface IUserRepository : IMasterRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByMobileAsync(string mobile, CancellationToken cancellationToken = default);
    Task<User?> GetByUserNameOrEmailAsync(string userNameOrEmail, CancellationToken cancellationToken = default);
    Task<UserAddress?> GetAddressByIdAsync(int userId, int addressId, CancellationToken cancellationToken = default);
    Task<ICollection<UserAddress>> GetAddressAsync(int userId, CancellationToken cancellationToken = default);
    Task DeleteAddressAsync(UserAddress userAddress, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
