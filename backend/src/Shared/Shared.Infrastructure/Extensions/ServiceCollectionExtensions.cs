using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Interfaces.Services;
using Shared.Infrastructure.Options;
using Shared.Infrastructure.Services;

namespace Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, string appConfigSectionName)
    {
        //Configure options
        services.AddOptions<EmailOptions>().BindConfiguration($"{appConfigSectionName}:{EmailOptions.SectionName}").ValidateOnStart();
        services.AddOptions<TokenOptions>().BindConfiguration($"{appConfigSectionName}:{TokenOptions.SectionName}").ValidateOnStart();
        services.AddOptions<OtpOptions>().BindConfiguration($"{appConfigSectionName}:{OtpOptions.SectionName}").ValidateOnStart();
        services.AddOptions<BrandingOptions>().BindConfiguration($"{appConfigSectionName}:{BrandingOptions.SectionName}").ValidateOnStart();
        services.AddOptions<FrontendOptions>().BindConfiguration($"{appConfigSectionName}:{FrontendOptions.SectionName}").ValidateOnStart();

        // register IHttpContextAccessor if needed for auditing
        services.AddHttpContextAccessor();

        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IEmailTemplateRenderer, EmailTemplateRenderer>();
        services.AddScoped<IEmailTemplateContextProvider, EmailTemplateContextProvider>();
        services.AddScoped<ISmsSender, TestSmsSender>();
        services.AddScoped<IAppUrlProvider, AppUrlProvider>();
        services.AddScoped<IOtpGenerator, OtpGenerator>();

        return services;
    }
}
