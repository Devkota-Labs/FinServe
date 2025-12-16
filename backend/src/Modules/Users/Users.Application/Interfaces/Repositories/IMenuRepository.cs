using Shared.Application.Interfaces.Repositories;
using Users.Domain.Entities;

namespace Users.Application.Interfaces.Repositories;

public interface IMenuRepository : IMasterRepository<Menu>
{
    Task<ICollection<Menu>?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}
