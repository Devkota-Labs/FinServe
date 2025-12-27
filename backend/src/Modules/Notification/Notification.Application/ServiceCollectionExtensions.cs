using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Dispatchers;
using Notification.Application.Interfaces;
using Notification.Application.Options;
using Notification.Application.Orchestrator;
using Notification.Application.Resolvers;
using Notification.Application.Retry;
using Notification.Application.Services;

namespace Notification.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationApplication(this IServiceCollection services, string appConfigSectionName)
    {
        //Configure options
        services.AddOptions<ScheduledJobsOptions>().BindConfiguration($"{appConfigSectionName}:{ScheduledJobsOptions.SectionName}").ValidateOnStart();

        //Configure Services
        services.AddScoped<INotificationOrchestrator, NotificationOrchestrator>();
        services.AddScoped<INotificationDispatcher, NotificationDispatcher>();
        services.AddScoped<INotificationChannelResolver, DefaultNotificationChannelResolver>();
        services.AddScoped<INotificationRetryService, NotificationRetryService>();
        services.AddScoped<INotificationQueryService, NotificationQueryService>();
        services.AddScoped<INotificationDedupRuleResolver, NotificationDedupRuleResolver>();
        services.AddScoped<IPasswordReminderService, PasswordReminderService>();

        return services;
    }
}
