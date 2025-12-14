using Location.Application;
using Location.Application.Interfaces.Repositories;
using Location.Infrastructure.Db;
using Location.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Location.Infrastructure.Module;

public static class LocationModule
{
    public static IServiceCollection AddLocationModule(this IServiceCollection services, IConfiguration config)
    {
        // Module-wise DbContext
        var conn = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Missing DefaultConnection");
        services.AddDbContext<LocationDbContext>(options =>
        {
            options.UseMySql(conn, ServerVersion.AutoDetect(conn));
        });

        // Register Repositories
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IStateRepository, StateRepository>();
        services.AddScoped<ICityRepository, CityRepository>();

        services.AddLocationApplication();

        return services;
    }

    public static IApplicationBuilder AddLocationMigrations(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LocationDbContext>();
        db.Database.Migrate();
        return app;
    }
}