using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;
using Users.Infrastructure.Seed;

namespace Users.Infrastructure.EfConfig;

internal sealed class UserAddressEfConfig : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.ToTable("UserAddresses");

        // Primary key
        builder.HasKey(a => a.Id);

        // Foreign key
        builder.Property(a => a.UserId)
            .IsRequired();

        // Address fields
        builder.Property(a => a.AddressLine1)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(a => a.PinCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.CountryId)
            .IsRequired();

        builder.Property(a => a.StateId)
            .IsRequired();

        builder.Property(a => a.CityId)
            .IsRequired();

        builder.Property(a => a.IsPrimary)
            .IsRequired();

        // Indexes (IMPORTANT)
        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => new { a.UserId, a.IsPrimary });

        builder.HasIndex(a => new { a.UserId })
            .HasFilter("[IsPrimary] = 1")
            .IsUnique();

        UserAddressSeeder.Seed(builder);
    }
}
