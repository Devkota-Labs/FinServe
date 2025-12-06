using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Database;

namespace Location.Infrastructure.Db;

internal sealed class LocationDbContextFactory : BaseDbContextFactory<LocationDbContext>
{
    public override string ModuleName => nameof(Location);
    public override LocationDbContext CreateNewInstance(DbContextOptions<LocationDbContext> options)
    {
        return new LocationDbContext(options);
    }
}
