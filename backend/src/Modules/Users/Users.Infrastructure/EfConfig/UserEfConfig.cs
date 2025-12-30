using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;
using Users.Infrastructure.Seed;

namespace Users.Infrastructure.EfConfig;

internal sealed class UserEfConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
        builder.Property(u => u.UserName).IsRequired().HasMaxLength(20);
        builder.Property(u => u.Gender)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany(u => u.UserRoles)
               .WithOne(ur => ur.User)
               .HasForeignKey(ur => ur.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        // Relationships
        builder.HasMany(u => u.Addresses)
            .WithOne()
            .HasForeignKey(a => a.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Backing field mapping (IMPORTANT)
        builder.Navigation(u => u.Addresses)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Mobile).IsUnique();

        UserSeeder.Seed(builder);
    }
}
