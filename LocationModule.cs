using Microsoft.Extensions.DependencyInjection;
using Modules.Location.Application.Services;
using Modules.Location.Infrastructure.Repositories;

namespace Modules.Location;

public static class LocationModule
{
    public static IServiceCollection AddLocationModule(this IServiceCollection services)
    {
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IStateRepository, StateRepository>();
        services.AddScoped<ICityRepository, CityRepository>();

        services.AddScoped<ILocationService, LocationService>();

        return services;
    }
}
