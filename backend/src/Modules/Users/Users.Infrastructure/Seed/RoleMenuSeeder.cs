using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.Seed;

internal static class RoleMenuSeeder
{
    public static void Seed(EntityTypeBuilder<RoleMenu> builder)
    {
        builder.HasData
            (
            new RoleMenu { Id = 1, RoleId = 1, MenuId = 9 }
            );
    }
}
