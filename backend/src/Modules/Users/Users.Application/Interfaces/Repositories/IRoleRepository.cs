using Shared.Application.Interfaces;
using Users.Domain.Entities;

namespace Users.Application.Interfaces.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<ICollection<Menu>> GetMenusAsync(int roleId);
    Task AssignMenusAsync(int roleId, ICollection<int> menuIds);
}
