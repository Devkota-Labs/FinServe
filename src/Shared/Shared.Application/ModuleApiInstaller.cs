using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Application;

public static class ModuleApiInstaller
{
    public static IServiceCollection AddModuleApiControllers(this IServiceCollection services)
    {
        var assemblies = Directory.GetFiles(AppContext.BaseDirectory, "*.Api.dll")
            .Select(Assembly.LoadFrom);

        foreach (var assembly in assemblies)
        {
            services.AddControllers().AddApplicationPart(assembly);
        }

        return services;
    }
}
