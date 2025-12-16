using Shared.Application.Interfaces.Services;
using Users.Application.Dtos.Menu;

namespace Users.Application.Interfaces.Services;

public interface IMenuService : IService<MenuDto, CreateMenuDto, UpdateMenuDto>
{
}
