using Location.Domain.Entities;
using Location.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Location.Infrastructure.EfConfig;

internal sealed class CountryEfConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("Countries");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.IsoCode).IsRequired().HasMaxLength(5);
        builder.Property(x => x.MobileCode).IsRequired().HasMaxLength(10);

        builder.HasMany(x => x.States)
               .WithOne(x => x.Country)
               .HasForeignKey(x => x.CountryId)
               .OnDelete(DeleteBehavior.Cascade);

        CountrySeeder.Seed(builder);
    }
}
