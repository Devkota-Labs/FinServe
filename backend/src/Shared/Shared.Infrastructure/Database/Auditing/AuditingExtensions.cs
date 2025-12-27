using Microsoft.EntityFrameworkCore;
using Shared.Application.Interfaces.Entities;
using Shared.Common.Utils;

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
                entry.Entity.CreatedTime = DateTimeUtil.Now;
                entry.Entity.LastUpdatedBy = user;
                entry.Entity.LastUpdatedTime = DateTimeUtil.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastUpdatedBy = user;
                entry.Entity.LastUpdatedTime = DateTimeUtil.Now;

                // Prevent overwriting Create fields
                entry.Property(x => x.CreatedTime).IsModified = false;
                entry.Property(x => x.CreatedBy).IsModified = false;
            }
        }
    }
}
