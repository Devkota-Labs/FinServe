using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Api;
using Users.Application.Dtos.Menu;
using Users.Application.Interfaces.Services;

namespace Users.Api.Controllers;

[ApiVersion("1.0")]
[Authorize]
[Authorize(Roles = "Admin")]
public sealed class MenusController(ILogger logger, IMenuService menuService)
    : BaseApiController(logger.ForContext<MenusController>())
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var serviceResponse = await menuService.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await menuService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateMenuDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var serviceResponse = await menuService.CreateAsync(dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Put(int id, UpdateMenuDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var serviceResponse = await menuService.UpdateAsync(id, dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await menuService.DeleteAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }
}