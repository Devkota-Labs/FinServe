using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;
using System.Data;
using Users.Application.Dtos.Menu;
using Users.Application.Dtos.Role;
using Users.Application.Interfaces.Repositories;
using Users.Application.Interfaces.Services;
using Users.Domain.Entities;

namespace Users.Application.Services;

internal sealed class RoleService(ILogger logger, IRoleRepository repo)
    : BaseService(logger.ForContext<RoleService>(), null), IRoleService
{
    public async Task<Result<ICollection<RoleDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repo.GetAllAsync(cancellationToken).ConfigureAwait(false);

        var result = entities.Select(Map).ToList();

        return Result.Ok<ICollection<RoleDto>>(result);
    }

    public async Task<Result<RoleDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entities = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entities == null)
            return Result.Fail<RoleDto>("Role not found");

        return Result.Ok(Map(entities));
    }

    public async Task<Result<RoleDto>> CreateAsync(CreateRoleDto dto, CancellationToken cancellationToken)
    {
        var exists = await repo.GetByNameAsync(dto.Name, cancellationToken).ConfigureAwait(false);

        if (exists is not null)
        {
            return Result.Fail<RoleDto>($"Role with name {exists.Name} already exists.");
        }

        var newEntity = new Role
        {
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive
        };

        await repo.AddAsync(newEntity, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Role created successfully.", Map(newEntity));
    }

    public async Task<Result<RoleDto>> UpdateAsync(int id, UpdateRoleDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<RoleDto>("Role not found.");

        if (dto.Name is not null)
        {
            var exists = await repo.GetByNameAsync(dto.Name, cancellationToken).ConfigureAwait(false);

            if (exists is not null)
            {
                return Result.Fail<RoleDto>($"Role with name {exists.Name} already exists.");
            }

            entity.Name = dto.Name;
        }

        if (dto.Description is not null) entity.Description = dto.Description;
        if (dto.IsActive is not null) entity.IsActive = dto.IsActive.Value;

        await repo.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Role updated successfully", Map(entity));
    }

    public async Task<Result<RoleDto>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<RoleDto>("Role not found.");

        await repo.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Role deleted successfully.", Map(entity));
    }

    public async Task<Result<ICollection<MenuDto>>> GetMenus(int roleId, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(roleId, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<ICollection<MenuDto>>("Role not found.");

        if (entity.RoleMenus is null)
        {
            return Result.Fail<ICollection<MenuDto>>($"No menus are assigned to role {entity.Name}");
        }

        var menus = entity.RoleMenus.Select(roleMenu => new MenuDto(roleMenu.MenuId, roleMenu.Menu.Name, roleMenu.Menu.Route, roleMenu.Menu.Icon, roleMenu.Menu.Sequence)).ToList();

        return Result.Ok<ICollection<MenuDto>>(menus);
    }

    public async Task<Result<RoleDto>> AssignMenus(int roleId, AssignMenusDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(roleId, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<RoleDto>("Role not found.");

        await repo.AssignMenusAsync(roleId, dto.MenuIds, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Menus assigned successfully.", Map(entity));
    }

    private static RoleDto Map(Role role)
    {
        return new(role.Id, role.Name, role.Description, role.RoleMenus != null ? [.. role.RoleMenus.Select(rm => rm.Menu.Name)] : null);
    }
}