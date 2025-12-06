using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Common;

public static class SharedCommonModule
{
    public static IServiceCollection AddSharedCommonModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}
