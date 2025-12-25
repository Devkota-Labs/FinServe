using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.EfConfig;

public class UserNotificationEfConfig : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("UserNotifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Category)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Severity)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ActionType)
            .HasConversion<int>();

        builder.Property(x => x.ReferenceType)
            .HasConversion<int>();

        builder.Property(x => x.IsRead)
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.IsRead });
    }
}
