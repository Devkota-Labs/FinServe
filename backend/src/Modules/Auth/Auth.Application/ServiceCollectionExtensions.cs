using Auth.Application.Interfaces.Services;
using Auth.Application.Options;
using Auth.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Shared.Security.Configurations;

namespace Auth.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthApplication(this IServiceCollection services, string appConfigSectionName)
    {
        //Configure options
        services.AddOptions<PasswordPolicyOptions>().BindConfiguration($"{appConfigSectionName}:{SecurityOptions.SectionName}:{PasswordPolicyOptions.SectionName}").ValidateOnStart();
        services.AddOptions<ScheduledJobsOptions>().BindConfiguration($"{appConfigSectionName}:{ScheduledJobsOptions.SectionName}").ValidateOnStart();
        services.AddOptions<AdminOptions>().BindConfiguration($"{appConfigSectionName}:{AdminOptions.SectionName}").ValidateOnStart();
        services.AddOptions<ApiOptions>().BindConfiguration($"{appConfigSectionName}:{ApiOptions.SectionName}").ValidateOnStart();

        //Configure Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMfaService, MfaService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IPasswordHistoryService, PasswordHistoryService>();
        services.AddScoped<IPasswordReminderService, PasswordReminderService>();
        services.AddScoped<ILoginHistoryService, LoginHistoryService>();

        return services;
    }
}
