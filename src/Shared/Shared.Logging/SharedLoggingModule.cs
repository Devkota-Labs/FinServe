using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Logging;

public static class SharedLoggingModule
{
    public static IServiceCollection AddSharedLoggingModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<ILogProvider, SerilogProvider>();

        return services;
    }
}
