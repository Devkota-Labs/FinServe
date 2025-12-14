using Serilog;
using Shared.Application.Dtos;
using Shared.Application.Results;
using Shared.Common.Services;
using Users.Application.Dtos.Menu;
using Users.Application.Interfaces.Repositories;
using Users.Application.Interfaces.Services;
using Users.Domain.Entities;

namespace Users.Application.Services;

internal sealed class MenuService(ILogger logger, IMenuRepository repo)
    : BaseService(logger.ForContext<MenuService>(), null), IMenuService
{
    private List<MenuTreeDto> BuildMenuTree(ICollection<Menu> menus)
    {
        var lookup = menus.ToDictionary(m => m.Id, m => new MenuTreeDto(m.Id, m.Name, m.Route ?? "", m.Icon ?? "", m.Sequence)
        {
            Id = m.Id,
            Name = m.Name,
            Route = m.Route ?? "",
            Icon = m.Icon ?? "",
            Order = m.Sequence
        });

        List<MenuTreeDto> roots = new();

        foreach (var menu in menus)
        {
            if (menu.ParentId == null)
            {
                // Root menu
                roots.Add(lookup[menu.Id]);
            }
            else if (lookup.ContainsKey(menu.ParentId.Value))
            {
                // Child menu
                lookup[menu.ParentId.Value].Children.Add(lookup[menu.Id]);
            }
        }

        // Sort children
        foreach (var item in lookup.Values)
        {
            item.Children = [.. item.Children.OrderBy(c => c.Order)];
        }

        // Sort roots
        return [.. roots.OrderBy(r => r.Order)];
    }

    public async Task<Result<ICollection<MenuDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repo.GetAllAsync(cancellationToken).ConfigureAwait(false);

        var result = entities.Select(Map).ToList();

        return Result<ICollection<MenuDto>>.Ok(result);
    }

    public async Task<Result<MenuDto?>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entities = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entities == null)
            return Result<MenuDto?>.Fail("Menu not found");

        return Result<MenuDto?>.Ok(Map(entities));
    }

    public async Task<Result<ICollection<MenuTreeDto>>> GetUserMenusAsync(int userId, CancellationToken cancellationToken)
    {
        var menus = await repo.GetByUserIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (menus is null)
        {
            return Result<ICollection<MenuTreeDto>>.Fail($"No menus are assigned to user {userId}.");
        }

        //4 CONVERT TO TREE
        var menuTree = BuildMenuTree(menus);
        return Result<ICollection<MenuTreeDto>>.Ok(menuTree);
    }

    public async Task<Result<MenuDto>> CreateAsync(CreateMenuDto dto, CancellationToken cancellationToken)
    {
        var exists = await repo.GetByNameAsync(dto.Name, cancellationToken).ConfigureAwait(false);

        if (exists is not null)
        {
            return Result<MenuDto>.Fail($"Menu with name {exists.Name} already exists.");
        }

        var newEntity = new Menu
        {
            Name = dto.Name,
            ParentId = dto.ParentMenuId,
            Route = dto.Route,
            Icon = dto.Icon,
            Sequence = dto.Order
        };

        await repo.AddAsync(newEntity, cancellationToken).ConfigureAwait(false);

        return Result<MenuDto>.Ok("Menu created successfully.", Map(newEntity));
    }

    public async Task<Result<MenuDto>> UpdateAsync(int id, UpdateMenuDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result<MenuDto>.Fail("Menu not found.");

        if (dto.Name is not null)
        {
            var exists = await repo.GetByNameAsync(dto.Name, cancellationToken).ConfigureAwait(false);

            if (exists is not null)
            {
                return Result<MenuDto>.Fail($"Menu with name {exists.Name} already exists.");
            }

            entity.Name = dto.Name;
        }

        if (dto.ParentMenuId is not null) entity.ParentId = dto.ParentMenuId;
        if (dto.Route is not null) entity.Route = dto.Route;
        if (dto.Icon is not null) entity.Icon = dto.Icon;
        if (dto.Order is not null) entity.Sequence = dto.Order.Value;

        await repo.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result<MenuDto>.Ok("Menu updated successfully", Map(entity));
    }

    public async Task<Result<MenuDto>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result<MenuDto>.Fail("Menu not found.");

        await repo.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result<MenuDto>.Ok("Menu deleted successfully.", Map(entity));
    }

    private static MenuDto Map(Menu menu)
    {
        return new(menu.Id, menu.Name, menu.Icon, menu.Route, menu.Sequence);
    }
}