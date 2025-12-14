using Shared.Application.Interfaces;
using Users.Domain.Entities;

namespace Users.Application.Interfaces.Repositories;

public interface IRoleRepository : IMasterRepository<Role>
{
    Task<ICollection<Menu>> GetMenusAsync(int roleId, CancellationToken cancellationToken = default);
    Task AssignMenusAsync(int roleId, ICollection<int> menuIds, CancellationToken cancellationToken = default);
}
