using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Interfaces.Services;
using Shared.Infrastructure.Options;
using Shared.Infrastructure.Services;

namespace Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Configure options
        services.Configure<EmailOptions>(configuration.GetSection("AppConfig:Email"));
        services.Configure<TokenOptions>(configuration.GetSection("AppConfig:Token"));
        services.Configure<OtpOptions>(configuration.GetSection("AppConfig:Otp"));

        // register IHttpContextAccessor if needed for auditing
        services.AddHttpContextAccessor();

        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IEmailTemplateRenderer, EmailTemplateRenderer>();
        services.AddScoped<ISmsSender, TestSmsSender>();
        services.AddScoped<IAppUrlProvider, AppUrlProvider>();
        services.AddScoped<IOtpGenerator, OtpGenerator>();

        return services;
    }
}
