using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.Seed;

internal static class RoleSeeder
{
    public static void Seed(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
                new Role { Id = 1, Name = "Default", Description = "Default Role", IsActive = true },
                new Role { Id = 2, Name = "Admin", Description = "Platform administrator", IsActive = true },
                new Role { Id = 3, Name = "Employee", Description = "Internal employee", IsActive = true },
                new Role { Id = 4, Name = "Dealer", Description = "Car dealer", IsActive = true },
                new Role { Id = 5, Name = "Banker", Description = "Bank representative", IsActive = true },
                new Role { Id = 6, Name = "Customer", Description = "End customer", IsActive = true }
        );
    }
}
