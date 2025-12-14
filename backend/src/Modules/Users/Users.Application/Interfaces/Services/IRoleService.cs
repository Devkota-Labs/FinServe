using Shared.Application.Interfaces;
using Shared.Application.Results;
using Users.Application.Dtos.Menu;
using Users.Application.Dtos.Role;

namespace Users.Application.Interfaces.Services;

public interface IRoleService : IService<RoleDto, CreateRoleDto, UpdateRoleDto>
{
    Task<Result<ICollection<MenuDto>?>> GetMenus(int roleId, CancellationToken cancellationToken);
    Task<Result<RoleDto>> AssignMenus(int roleId, AssignMenusDto dto, CancellationToken cancellationToken);
}
