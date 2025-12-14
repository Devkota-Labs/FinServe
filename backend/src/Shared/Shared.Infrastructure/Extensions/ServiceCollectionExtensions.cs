using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Interfaces;
using Shared.Infrastructure.Services;

namespace Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
    {
        // register IHttpContextAccessor if needed for auditing
        services.AddHttpContextAccessor();

        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<ISmsSender, TestSmsSender>();
        services.AddScoped<IAppUrlProvider, AppUrlProvider>();
        services.AddScoped<IOtpGenerator, OtpGenerator>();

        return services;
    }
}
