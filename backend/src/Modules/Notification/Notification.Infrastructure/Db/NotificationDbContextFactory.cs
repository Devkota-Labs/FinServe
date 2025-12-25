using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Database;

namespace Notification.Infrastructure.Db;

internal sealed class NotificationDbContextFactory : BaseDbContextFactory<NotificationDbContext>
{
    public override string ModuleName => nameof(Notification);
    public override NotificationDbContext CreateNewInstance(DbContextOptions<NotificationDbContext> options)
    {
        return new NotificationDbContext(options);
    }
}
