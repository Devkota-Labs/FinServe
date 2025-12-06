using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Database;

namespace Users.Infrastructure.Db;

internal sealed class UserDbContextFactory : BaseDbContextFactory<UserDbContext>
{
    public override string ModuleName => "User";
    public override UserDbContext CreateNewInstance(DbContextOptions<UserDbContext> options)
    {
        return new UserDbContext(options);
    }
}
