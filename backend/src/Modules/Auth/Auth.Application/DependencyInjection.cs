using Auth.Application.Interfaces.Services;
using Auth.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMfaService, MfaService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IPasswordHistoryService, PasswordHistoryService>();
        services.AddScoped<IPasswordReminderService, PasswordReminderService>();
        services.AddScoped<ILoginHistoryService, LoginHistoryService>();

        return services;
    }
}
