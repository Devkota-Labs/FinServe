using Microsoft.Extensions.DependencyInjection;
using Users.Application.Interfaces.Services;
using Users.Application.Services;

namespace Users.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IMenuService, MenuService>();

        return services;
    }
}
