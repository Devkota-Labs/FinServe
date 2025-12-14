using Serilog;
using Shared.Application.Dtos;
using Shared.Common.Services;
using Users.Application.Interfaces.Repositories;
using Users.Application.Interfaces.Services;
using Users.Domain.Entities;

namespace Users.Infrastructure.Services;

internal sealed class MenuReadService(ILogger logger, IMenuRepository menuRepository)
    : BaseService(logger.ForContext<MenuReadService>(), null), IMenuReadService
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

    public async Task<ICollection<MenuTreeDto>> GetUserMenusAsync(int userId, CancellationToken cancellationToken = default)
    {
        var menus = await menuRepository.GetByUserIdAsync(userId, cancellationToken).ConfigureAwait(false);

        //4 CONVERT TO TREE
        var menuTree = BuildMenuTree(menus);
        return menuTree;
    }
}