using Microsoft.EntityFrameworkCore;
using Notification.Domain.Entities;
using Shared.Infrastructure.Database;

namespace Notification.Infrastructure.Db;

internal sealed class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : BaseDbContext(options)
{
    public DbSet<UserNotification> UserNotifications => Set<UserNotification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var module = nameof(Notification);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);

        // ⭐ Auto-prefix all table names
        ApplyModuleTablePrefix(modelBuilder, module);
    }
}
