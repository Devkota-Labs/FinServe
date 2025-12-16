using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Database;

namespace Auth.Infrastructure.Db;

internal sealed class AuthDbContextFactory : BaseDbContextFactory<AuthDbContext>
{
    public override string ModuleName => nameof(Auth);
    public override AuthDbContext CreateNewInstance(DbContextOptions<AuthDbContext> options)
    {
        return new AuthDbContext(options);
    }
}
