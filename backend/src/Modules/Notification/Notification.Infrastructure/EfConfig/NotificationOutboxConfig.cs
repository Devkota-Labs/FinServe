using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.EfConfig;

public sealed class NotificationOutboxConfig : IEntityTypeConfiguration<NotificationOutbox>
{
    public void Configure(EntityTypeBuilder<NotificationOutbox> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("Outbox");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Status);
    }
}
