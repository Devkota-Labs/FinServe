using Microsoft.Extensions.DependencyInjection;

namespace Shared.Common;

public static class SharedCommonModule
{
    public static IServiceCollection AddSharedCommonModule(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}
