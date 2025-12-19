using Location.Domain.Entities;
using Shared.Application.Interfaces.Repositories;

namespace Location.Application.Interfaces.Repositories;

public interface IStateRepository : IMasterRepository<State>
{
    Task<ICollection<State>> GetByCountryAsync(int countryId, CancellationToken cancellationToken = default);
}
