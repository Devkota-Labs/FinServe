using Location.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Database;

namespace Location.Infrastructure.Db;

internal sealed class LocationDbContext(DbContextOptions<LocationDbContext> options) : BaseDbContext(options)
{
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<State> States => Set<State>();
    public DbSet<City> Cities => Set<City>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var module = nameof(Location);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocationDbContext).Assembly);

        // ⭐ Auto-prefix all table names
        ApplyModuleTablePrefix(modelBuilder, module);
    }
}
