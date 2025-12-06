using Location.Domain.Entities;
using Shared.Application.Interfaces;

namespace Location.Application.Interfaces.Repositories;

public interface ICityRepository : IRepository<City>
{
    Task<List<City>?> GetByStateAsync(int stateId);
}