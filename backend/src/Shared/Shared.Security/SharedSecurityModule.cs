using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Security.Configurations;

namespace Shared.Security;

public static class SharedSecurityModule
{
    public static IServiceCollection AddSharedSecurityModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName).ValidateOnStart();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
