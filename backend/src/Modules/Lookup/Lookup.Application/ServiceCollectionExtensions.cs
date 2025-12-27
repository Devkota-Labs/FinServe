using Lookup.Application.Interfaces.Services;
using Lookup.Application.Lookups;
using Lookup.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lookup.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLookupApplication(this IServiceCollection services)
    {
        services.AddSingleton<ILookupProvider, EnumLookupProvider>();
        services.AddSingleton<GenderLookup>();
        services.AddSingleton<AddressTypeLookup>();

        return services;
    }
}
