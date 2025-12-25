using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Adapters;
using Notification.Application.Alerts;
using Notification.Application.Handlers;
using Notification.Application.Interfaces;
using Notification.Application.Options;
using Notification.Application.Orchestrator;
using Notification.Application.Services;
using Shared.Security;

namespace Notification.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationApplication(this IServiceCollection services, string appConfigSectionName)
    {
        //Configure options
        services.AddOptions<ScheduledJobsOptions>().BindConfiguration($"{appConfigSectionName}:{ScheduledJobsOptions.SectionName}").ValidateOnStart();

        //Configure Services
        services.AddScoped<IUserNotificationService, UserNotificationService>();
        services.AddScoped<NotificationOrchestrator>();
        services.AddScoped<LoginAlertService>();
        services.AddScoped<ILoginAlertNotifier, LoginAlertNotifier>();
        services.AddScoped<LoginAlertRule>();
        services.AddScoped<EmailChangedNotificationHandler>();
        services.AddScoped<IEmailChangedNotifier, EmailChangedNotifier>();
        services.AddScoped<EmailVerifiedNotificationHandler>();
        services.AddScoped<IEmailVerifiedNotifier, EmailVerifiedNotifier>();
        services.AddScoped<PasswordChangedNotificationHandler>();
        services.AddScoped<IPasswordChangedNotifier, PasswordChangedNotifier>();
        services.AddScoped<PasswordResetRequestedNotificationHandler>();        
        services.AddScoped<IPasswordResetRequestedNotifier, PasswordResetRequestedNotifier>();
        services.AddScoped<PasswordResetSuccessNotificationHandler>();
        services.AddScoped<IPasswordResetSuccessNotifier, PasswordResetSuccessNotifier>();
        services.AddScoped<UserApprovedNotificationHandler>();
        services.AddScoped<IUserApprovedNotifier, UserApprovedNotifier>();
        services.AddScoped<UserUnlockedNotificationHandler>();
        services.AddScoped<IUserUnlockedNotifier, UserUnlockedNotifier>();
        services.AddScoped<IPasswordReminderService, PasswordReminderService>();

        return services;
    }
}
