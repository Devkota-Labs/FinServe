using Location.Domain.Entities;
using Shared.Application.Interfaces;

namespace Location.Application.Interfaces.Repositories;

public interface IStateRepository : IRepository<State>
{
    Task<List<State>?> GetByCountryAsync(int countryId);
}
