using Location.Application.Dtos;
using Shared.Application.Interfaces;
using Shared.Application.Results;

namespace Location.Application.Interfaces.Services;

public interface ICityService : IService<CityDto, CreateCityDto, UpdateCityDto>
{
    Task<Result<ICollection<CityDto>?>> GetByStateAsync(int stateId, CancellationToken cancellationToken);
}
