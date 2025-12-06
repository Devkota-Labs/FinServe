using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Api;
using Users.Application.Dtos.Menu;
using Users.Application.Dtos.Role;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Entities;

namespace Users.Api.Controllers;

[ApiVersion("1.0")]
//[Authorize]
//[Authorize(Roles = "Admin")]
public sealed class RolesController(ILogger logger, IRoleRepository RoleRepository) : BaseApiController(logger.ForContext<RolesController>())
{
    private readonly IRoleRepository _roleRepository = RoleRepository;

    private RoleDto MapToDto(Role role) =>
        new(role.Id, role.Name, role.Description, [.. role.RoleMenus.Select(rm => rm.Menu.Name)]);

    private MenuDto MapToMenuDto(RoleMenu roleMenu) =>
        new(roleMenu.MenuId, roleMenu.Menu.Name, roleMenu.Menu.Route, roleMenu.Menu.Icon, roleMenu.Menu.Sequence);

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _roleRepository.GetAllAsync().ConfigureAwait(false);

        var roles = data.Select(MapToDto);

        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (role == null)
            return NotFound($"Role not found with id {id}");

        return Ok(MapToDto(role));
    }

    //GET Menus for a Role
    [HttpGet("menus/{roleId}")]
    public async Task<IActionResult> GetMenus(int roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId).ConfigureAwait(false);

        if (role is null)
            return NotFound($"Role not found with id {roleId}");

        if (role.RoleMenus is null)
        {
            return Ok($"No menus are assigned to role {role.Name}", role);
        }

        var menus = role.RoleMenus.Select(MapToMenuDto);

        return Ok(menus);
    }

    //Assign Menus to a Role
    [HttpPost("menus/{roleId}")]
    public async Task<IActionResult> AssignMenus(int roleId, AssignMenusDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var role = await _roleRepository.GetByIdAsync(roleId).ConfigureAwait(false);

        if (role is null)
            return NotFound($"Role not found with id {roleId}");

        await _roleRepository.AssignMenusAsync(role.Id, dto.MenuIds).ConfigureAwait(false);

        return Ok("Menus assigned successfully", MapToDto(role));
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateRoleDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var exists = await _roleRepository.GetByNameAsync(dto.Name).ConfigureAwait(false);

        if (exists is not null)
        {
            return BadRequest($"Role with name {exists.Name} already exists.");
        }

        var role = new Role
        {
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive
        };

        await _roleRepository.AddAsync(role).ConfigureAwait(false);

        return Created("Role created.", MapToDto(role));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Put(int id, UpdateRoleDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var role = await _roleRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (role == null)
            return NotFound($"Role not found with id {id}");

        if (dto.Name is not null)
        {
            var exists = await _roleRepository.GetByNameAsync(dto.Name).ConfigureAwait(false);

            if (exists is not null)
            {
                return BadRequest($"Role with name {exists.Name} already exists.");
            }

            role.Name = dto.Name;
        }

        if (dto.Description is not null) role.Description = dto.Description;
        if (dto.IsActive is not null) role.IsActive = dto.IsActive.Value;

        await _roleRepository.UpdateAsync(role).ConfigureAwait(false);

        return Ok("Role updated.", MapToDto(role));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (role == null)
            return NotFound($"Role not found with id {id}");

        await _roleRepository.DeleteAsync(role).ConfigureAwait(false);

        return Ok("Role deleted.", MapToDto(role));
    }
}