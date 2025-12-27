using Microsoft.EntityFrameworkCore;
using Notification.Domain.Entities;
using Shared.Infrastructure.Database;

namespace Notification.Infrastructure.Db;

internal sealed class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : BaseDbContext(options)
{
    public DbSet<InAppNotification> InAppNotifications => Set<InAppNotification>();
    public DbSet<NotificationOutbox> Outbox => Set<NotificationOutbox>();
    public DbSet<NotificationDeduplication> Deduplications => Set<NotificationDeduplication>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var module = "Notifications";

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);

        // ⭐ Auto-prefix all table names
        ApplyModuleTablePrefix(modelBuilder, module);
    }
}
