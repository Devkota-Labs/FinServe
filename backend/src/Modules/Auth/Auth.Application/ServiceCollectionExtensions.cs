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
        services.AddOptions<AdminOptions>().BindConfiguration($"{appConfigSectionName}:{AdminOptions.SectionName}").ValidateOnStart();
        services.AddOptions<ApiOptions>().BindConfiguration($"{appConfigSectionName}:{ApiOptions.SectionName}").ValidateOnStart();
        services.AddOptions<ReservedUsernameOptions>().BindConfiguration($"{appConfigSectionName}:{ReservedUsernameOptions.SectionName}").ValidateOnStart();

        //Configure Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMfaService, MfaService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IPasswordHistoryService, PasswordHistoryService>();
        services.AddScoped<ILoginHistoryService, LoginHistoryService>();
        services.AddScoped<ILoginRiskService, LoginRiskService>();
        services.AddScoped<IUserNamePolicyService, UserNamePolicyService>();
        services.AddScoped<IUserNameAvailabilityService, UserNameAvailabilityService>();

        return services;
    }
}
