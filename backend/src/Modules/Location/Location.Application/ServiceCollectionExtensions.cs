using Location.Application.Interfaces.Services;
using Location.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Location.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocationApplication(this IServiceCollection services)
    {
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IStateService, StateService>();
        services.AddScoped<ICityService, CityService>();

        return services;
    }
}
