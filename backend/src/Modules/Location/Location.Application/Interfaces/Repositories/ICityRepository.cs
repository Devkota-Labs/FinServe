using Location.Domain.Entities;
using Shared.Application.Interfaces.Repositories;

namespace Location.Application.Interfaces.Repositories;

public interface ICityRepository : IMasterRepository<City>
{
    Task<List<City>?> GetByStateAsync(int stateId, CancellationToken cancellationToken = default);
}