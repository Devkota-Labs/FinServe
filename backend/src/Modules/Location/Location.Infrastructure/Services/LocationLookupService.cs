using Location.Application.Dtos;
using Location.Application.Interfaces.Services;
using Location.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Common.Services;

namespace Location.Infrastructure.Services;

internal sealed class LocationLookupService(ILogger logger, LocationDbContext db)
    : BaseService(logger.ForContext<LocationLookupService>(), null), ILocationLookupService
{
    public async Task<LocationDto> GetLocationNamesAsync(int countryId, int stateId, int cityId, CancellationToken cancellationToken = default)
    {
        var country = await db.Countries
            .Where(x => x.Id == countryId)
            .Select(x => x.Name)
            .FirstAsync(cancellationToken).ConfigureAwait(false);

        var state = await db.States
            .Where(x => x.Id == stateId)
            .Select(x => x.Name)
            .FirstAsync(cancellationToken).ConfigureAwait(false);

        var city = await db.Cities
            .Where(x => x.Id == cityId)
            .Select(x => x.Name)
            .FirstAsync(cancellationToken).ConfigureAwait(false);

        return new LocationDto(countryId, country, stateId, state, cityId, city);
    }
}