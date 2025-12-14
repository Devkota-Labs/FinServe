using Shared.Application.Dtos;

namespace Users.Application.Interfaces.Services;

public interface IMenuReadService
{
    Task<ICollection<MenuTreeDto>> GetUserMenusAsync(int userId, CancellationToken cancellationToken = default);
}
