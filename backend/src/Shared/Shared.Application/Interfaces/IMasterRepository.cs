using Shared.Domain;

namespace Shared.Application.Interfaces;

public interface IMasterRepository<T> : IRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
