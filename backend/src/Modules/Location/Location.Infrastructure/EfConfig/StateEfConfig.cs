using Location.Domain.Entities;
using Location.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Location.Infrastructure.EfConfig;

internal sealed class StateEfConfig : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.ToTable("States");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);

        builder.HasMany(x => x.Cities)
               .WithOne(x => x.State)
               .HasForeignKey(x => x.StateId)
               .OnDelete(DeleteBehavior.Cascade);

        StateSeeder.Seed(builder);
    }
}
