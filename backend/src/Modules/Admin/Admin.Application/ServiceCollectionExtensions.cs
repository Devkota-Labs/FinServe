using Admin.Application.Interfaces.Services;
using Admin.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdminApplication(this IServiceCollection services)
    {
        services.AddScoped<IAdminService, AdminService>();

        return services;
    }
}
