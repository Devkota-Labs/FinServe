using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Channels;
using Notification.Infrastructure.Db;
using Notification.Infrastructure.HostedServices;
using Notification.Infrastructure.Rendering;
using Notification.Infrastructure.Repositories;
using Notification.Infrastructure.Services;
using Shared.Application.Interfaces.Services;

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
        services.AddScoped<IInAppNotificationRepository, InAppNotificationRepository>();
        services.AddScoped<INotificationOutboxRepository, NotificationOutboxRepository>();
        services.AddScoped<INotificationDeduplicationRepository, NotificationDeduplicationRepository>();

        //Register Services
        services.AddScoped<INotificationTemplateRenderer, NotificationTemplateRenderer>();
        services.AddScoped<INotificationDeduplicationService, NotificationDeduplicationService>();
        services.AddHostedService<PasswordReminderHostedService>();
        services.AddHostedService<NotificationRetryHostedService>();
        services.AddHostedService<NotificationEventConsumer>();
        //services.AddSignalR();
        // Module templates
        services.AddScoped<IEmailBodyTemplateProvider, EmbeddedNotificationEmailTemplateProvider>();

        //Register channels
        services.AddScoped<INotificationChannel, InAppNotificationChannel>();
        services.AddScoped<INotificationChannel, EmailNotificationChannel>();
        services.AddScoped<INotificationChannel, SmsNotificationChannel>();

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