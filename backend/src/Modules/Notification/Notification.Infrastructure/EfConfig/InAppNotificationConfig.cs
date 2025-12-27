using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.EfConfig;

public sealed class InAppNotificationConfig : IEntityTypeConfiguration<InAppNotification>
{
    public void Configure(EntityTypeBuilder<InAppNotification> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("InAppNotifications");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.UserId, x.IsRead });
    }
}
