using Microsoft.Extensions.DependencyInjection;
using Shared.Security.Configurations;

namespace Shared.Security;

public static class SharedSecurityModule
{
    public static IServiceCollection AddSharedSecurityModule(this IServiceCollection services, string appConfigSectionName)
    {
        //Configure options
        services.AddOptions<JwtOptions>().BindConfiguration($"{appConfigSectionName}:{JwtOptions.SectionName}").ValidateOnStart();
        services.AddOptions<SecurityOptions>().BindConfiguration($"{appConfigSectionName}:{SecurityOptions.SectionName}").ValidateOnStart();
        services.AddOptions<LockoutOptions>().BindConfiguration($"{appConfigSectionName}:{SecurityOptions.SectionName}:{LockoutOptions.SectionName}").ValidateOnStart();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
