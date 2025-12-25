using Shared.Application.Interfaces.Entities;

namespace Shared.Application.Interfaces.Repositories;

public interface IMasterRepository<T> : IRepository<T> where T : IBaseEntity
{
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
