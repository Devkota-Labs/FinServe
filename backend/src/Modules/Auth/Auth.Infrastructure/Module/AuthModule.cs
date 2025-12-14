using Auth.Application;
using Auth.Application.Interfaces.Repositories;
using Auth.Infrastructure.Db;
using Auth.Infrastructure.Jobs;
using Auth.Infrastructure.Repositories;
using Auth.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Security;

namespace Auth.Infrastructure.Module;

public static class AuthModule
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration config)
    {
        // Module-wise DbContext
        var conn = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Missing DefaultConnection");
        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseMySql(conn, ServerVersion.AutoDetect(conn));
        });

        // Register Repositories
        services.AddScoped<IOtpRepository, OtpRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IPasswordHistoryRepository, PasswordHistoryRepository>();

        //Register Services        
        services.AddScoped<IPasswordPolicyService, PasswordPolicyService>();        
        services.AddHostedService<PasswordReminderHostedService>();

        services.AddAuthApplication();

        return services;
    }

    public static IApplicationBuilder AddAuthMigrations(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        db.Database.Migrate();
        return app;
    }
}