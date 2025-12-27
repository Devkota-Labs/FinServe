using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;
using Users.Infrastructure.Seed;

namespace Users.Infrastructure.EfConfig;

internal sealed class RoleMenuEfConfig : IEntityTypeConfiguration<RoleMenu>
{
    public void Configure(EntityTypeBuilder<RoleMenu> builder)
    {
        builder.ToTable("RoleMenus");
        builder.HasKey(x => x.Id);
        builder.HasIndex(rm => new { rm.RoleId, rm.MenuId })
            .IsUnique();

        builder
            .HasOne(x => x.Role)
            .WithMany(u => u.RoleMenus)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Menu)
            .WithMany(r => r.RoleMenus)
            .HasForeignKey(x => x.MenuId)
            .OnDelete(DeleteBehavior.Restrict);

        RoleMenuSeeder.Seed(builder);
    }
}
