using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Api;
using Users.Application.Dtos.Menu;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Entities;

namespace Users.Api.Controllers;

[ApiVersion("1.0")]
//[Authorize]
//[Authorize(Roles = "Admin")]
public sealed class MenusController(ILogger logger, IMenuRepository MenuRepository) : BaseApiController(logger.ForContext<MenusController>())
{
    private readonly IMenuRepository _menuRepository = MenuRepository;

    private MenuDto MapToDto(Menu menu) =>
        new(menu.Id, menu.Name, menu.Icon, menu.Route, menu.Sequence);

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _menuRepository.GetAllAsync().ConfigureAwait(false);

        var menus = data.Select(MapToDto);

        return Ok(menus);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var menu = await _menuRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (menu == null)
            return NotFound($"Menu not found with id {id}");

        return Ok(MapToDto(menu));
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateMenuDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var exists = await _menuRepository.GetByNameAsync(dto.Name).ConfigureAwait(false);

        if (exists is not null)
        {
            return BadRequest($"Menu with name {exists.Name} already exists.");
        }

        var menu = new Menu
        {
            Name = dto.Name,
            ParentId = dto.ParentMenuId,
            Route = dto.Route,
            Icon = dto.Icon,
            Sequence = dto.Order
        };

        await _menuRepository.AddAsync(menu).ConfigureAwait(false);

        return Created("Menu created.", MapToDto(menu));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Put(int id, UpdateMenuDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var menu = await _menuRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (menu == null)
            return NotFound($"Menu not found with id {id}");

        if (dto.Name is not null)
        {
            var exists = await _menuRepository.GetByNameAsync(dto.Name).ConfigureAwait(false);

            if (exists is not null)
            {
                return BadRequest($"Menu with name {exists.Name} already exists.");
            }

            menu.Name = dto.Name;
        }
        if (dto.ParentMenuId is not null) menu.ParentId = dto.ParentMenuId;
        if (dto.Route is not null) menu.Route = dto.Route;
        if (dto.Icon is not null) menu.Icon = dto.Icon;
        if (dto.Order is not null) menu.Sequence = dto.Order.Value;

        await _menuRepository.UpdateAsync(menu).ConfigureAwait(false);

        return Ok("Menu updated.", MapToDto(menu));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var menu = await _menuRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (menu == null)
            return NotFound($"Menu not found with id {id}");

        await _menuRepository.DeleteAsync(menu).ConfigureAwait(false);

        return Ok("Menu deleted.", MapToDto(menu));
    }
}