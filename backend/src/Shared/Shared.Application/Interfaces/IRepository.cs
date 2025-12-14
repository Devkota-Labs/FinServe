using Shared.Domain;

namespace Shared.Application.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
