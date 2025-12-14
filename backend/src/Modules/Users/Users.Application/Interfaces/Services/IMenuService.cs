using Shared.Application.Dtos;
using Shared.Application.Interfaces;
using Shared.Application.Results;
using Users.Application.Dtos.Menu;

namespace Users.Application.Interfaces.Services;

public interface IMenuService : IService<MenuDto, CreateMenuDto, UpdateMenuDto>
{
}
