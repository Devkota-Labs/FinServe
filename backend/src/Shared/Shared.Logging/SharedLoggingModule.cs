using Microsoft.Extensions.DependencyInjection;

namespace Shared.Logging;

public static class SharedLoggingModule
{
    public static IServiceCollection AddSharedLoggingModule(this IServiceCollection services)
    {
        services.AddSingleton<ILogProvider, SerilogProvider>();

        return services;
    }
}
