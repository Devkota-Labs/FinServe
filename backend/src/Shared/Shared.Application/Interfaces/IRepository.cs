using Shared.Domain;

namespace Shared.Application.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<bool> ExistsAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetByNameAsync(string name);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
