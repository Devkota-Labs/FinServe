using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.Seed;

internal static class UserRoleSeeder
{
    public static void Seed(EntityTypeBuilder<UserRole> builder)
    {
        var seedTime = new DateTime(2024, 1, 1);

        var adminUser = new UserRole
        {
            Id = 1,
            RoleId = 1,
            UserId = 1,
            CreatedTime = seedTime,
            CreatedBy = "Admin"
        };

        builder.HasData(adminUser);
    }
}