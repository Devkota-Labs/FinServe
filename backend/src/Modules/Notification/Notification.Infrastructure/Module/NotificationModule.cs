using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Db;
using Notification.Infrastructure.HostedServices;
using Notification.Infrastructure.Repositories;
using Notification.Infrastructure.Services;

namespace Notification.Infrastructure.Module;

public static class NotificationModule
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection services, string appConfigSectionName, IConfiguration config)
    {
        // Module-wise DbContext
        var conn = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Missing DefaultConnection");
        services.AddDbContext<NotificationDbContext>(options =>
        {
            options.UseMySql(conn, ServerVersion.AutoDetect(conn));
        });

        // Register Repositories
        services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();

        //Register Services
        services.AddScoped<IInAppNotificationService, InAppNotificationService>();
        services.AddScoped<IEmailNotificationService, EmailNotificationService>();
        services.AddScoped<INotificationDeduplicationService, NotificationDeduplicationService>();
        services.AddHostedService<PasswordReminderHostedService>();
        //services.AddSignalR();

        services.AddNotificationApplication(appConfigSectionName);

        return services;
    }

    public static IApplicationBuilder AddNotificationMigrations(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        db.Database.Migrate();

        return app;
    }
}