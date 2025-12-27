using Location.Application.Dtos;

namespace Location.Application.Interfaces.Services;

public interface ILocationLookupService
{
    Task<LocationDto> GetLocationNamesAsync(int countryId, int stateId, int cityId, CancellationToken cancellationToken = default);
}