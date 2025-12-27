using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.EfConfig;

public sealed class NotificationDeduplicationConfig
    : IEntityTypeConfiguration<NotificationDeduplication>
{
    public void Configure(EntityTypeBuilder<NotificationDeduplication> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("Deduplications");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x =>
            new
            {
                x.UserId,
                x.Type,
                x.Channel,
                x.DedupKey,
                x.CreatedAt
            });

        builder.Property(x => x.DedupKey)
            .HasMaxLength(200);
    }
}