using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
    {
        // register IHttpContextAccessor if needed for auditing
        services.AddHttpContextAccessor();

        return services;
    }
}
