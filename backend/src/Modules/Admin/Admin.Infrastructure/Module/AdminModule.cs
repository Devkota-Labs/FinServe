using Admin.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Infrastructure.Module;

public static class AdminModule
{
    public static IServiceCollection AddAdminModule(this IServiceCollection services, IConfiguration config)
    {
        // Register Repositories

        //Register Services        
        
        services.AddAdminApplication();

        return services;
    }

    public static IApplicationBuilder AddAdminMigrations(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app;
    }
}