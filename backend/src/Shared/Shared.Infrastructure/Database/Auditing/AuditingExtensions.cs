using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Database.Auditing;

internal static class AuditingExtensions
{
    public static void ApplyAuditInformation(this BaseDbContext context)
    {
        var user = context.CurrentUser ?? "System";

        var entries = context.ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = user;
                entry.Entity.CreatedTime = DateTime.UtcNow;
                entry.Entity.LastUpdatedBy = user;
                entry.Entity.LastUpdatedTime = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastUpdatedBy = user;
                entry.Entity.LastUpdatedTime = DateTime.UtcNow;

                // Prevent overwriting Create fields
                entry.Property(x => x.CreatedTime).IsModified = false;
                entry.Property(x => x.CreatedBy).IsModified = false;
            }
        }
    }
}
