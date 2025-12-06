using Microsoft.Extensions.DependencyInjection;
using Modules.Auth.Infrastructure.Repositories;
using Modules.Auth.Infrastructure.Services;
using Modules.Auth.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Auth.Application.Interfaces;

namespace Auth.Api
{
    public static class AuthModule
    {
        public static IServiceCollection AddAuthModule(this IServiceCollection services, string? connectionString = null)
        {
            // Register DbContext - if connectionString is null, assume main AppDbContext is used elsewhere
            if (!string.IsNullOrEmpty(connectionString))
            {
                services.AddDbContext<AuthDbContext>(opts => opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            }
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
