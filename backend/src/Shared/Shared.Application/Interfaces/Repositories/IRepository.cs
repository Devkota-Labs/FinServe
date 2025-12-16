using Shared.Application.Interfaces.Entities;

namespace Shared.Application.Interfaces.Repositories;

public interface IRepository<T> where T : IBaseEntity
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
