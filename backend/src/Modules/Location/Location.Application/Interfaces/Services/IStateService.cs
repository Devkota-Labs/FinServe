using Location.Application.Dtos;
using Shared.Application.Interfaces.Services;
using Shared.Application.Results;

namespace Location.Application.Interfaces.Services;

public interface IStateService : IService<StateDto, CreateStateDto, UpdateStateDto>
{
    Task<Result<ICollection<StateDto>>> GetByCountryAsync(int countryId, CancellationToken cancellationToken);
}
