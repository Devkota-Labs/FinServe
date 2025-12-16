using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Api;
using Users.Application.Dtos.Role;
using Users.Application.Interfaces.Services;

namespace Users.Api.Controllers;

[ApiVersion("1.0")]
[Authorize]
[Authorize(Roles = "Admin")]
public sealed class RolesController(ILogger logger, IRoleService roleService)
    : BaseApiController(logger.ForContext<RolesController>())
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var serviceResponse = await roleService.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await roleService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    //GET Menus for a Role
    [HttpGet("menus/{roleId}")]
    public async Task<IActionResult> GetMenus(int roleId, CancellationToken cancellationToken)
    {
        var serviceResponse = await roleService.GetMenus(roleId, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    //Assign Menus to a Role
    [HttpPost("menus/{roleId}")]
    public async Task<IActionResult> AssignMenus(int roleId, AssignMenusDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var serviceResponse = await roleService.AssignMenus(roleId, dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateRoleDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var serviceResponse = await roleService.CreateAsync(dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Put(int id, UpdateRoleDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var serviceResponse = await roleService.UpdateAsync(id, dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await roleService.DeleteAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }
}