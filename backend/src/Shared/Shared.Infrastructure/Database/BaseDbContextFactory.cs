using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Shared.Infrastructure.Database;

public abstract class BaseDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
{
    public abstract string ModuleName { get; }
    public abstract TContext CreateNewInstance(DbContextOptions<TContext> options);
    public TContext CreateDbContext(string[] args)
    {
        // 1. Build configuration manually
        // Dynamically locate the project folder (.csproj)
        var projectPath = DesignTimeProjectLocator.GetProjectPath<TContext>();
        var config = new ConfigurationBuilder()
            .SetBasePath(projectPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        // 2. Get connection string
        var connectionString = config.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string not found.");

        // 3. Create DbContext options
        var builder = new DbContextOptionsBuilder<TContext>();
        builder.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString),
            mySqlOptions =>
            {
                mySqlOptions.EnableRetryOnFailure();
            });

        return CreateNewInstance(builder.Options);
    }
}
