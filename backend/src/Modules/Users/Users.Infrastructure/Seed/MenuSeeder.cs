using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.Seed;

internal static class MenuSeeder
{
    public static void Seed(EntityTypeBuilder<Menu> builder)
    {
        var seedTime = new DateTime(2024, 1, 1);

        builder.HasData(
            new Menu { Id = 1, Name = "Profile", Route = "#", ParentId = null, Sequence = 1, CreatedBy = "Admin" },
            new Menu { Id = 2, Name = "Masters", Route = "#", ParentId = null, Sequence = 2, CreatedBy = "Admin" },
            new Menu { Id = 3, Name = "Users", Route = "#", ParentId = null, Sequence = 3, CreatedBy = "Admin" },
            //Profile Menus
            new Menu { Id = 9, Name = "View Profile", Route = "/profile", ParentId = 1, Sequence = 1, CreatedBy = "Admin" },
            new Menu { Id = 10, Name = "Change Password", Route = "/auth/change-password", ParentId = 1, Sequence = 2, CreatedBy = "Admin" },
            //Master Menus
            new Menu { Id = 4, Name = "Countries", Route = "/admin/masters/countries", ParentId = 2, Sequence = 1, CreatedBy = "Admin" },
            new Menu { Id = 5, Name = "States", Route = "/admin/masters/states", ParentId = 2, Sequence = 2, CreatedBy = "Admin" },
            new Menu { Id = 6, Name = "Cities", Route = "/admin/masters/cities", ParentId = 2, Sequence = 3, CreatedBy = "Admin" },
            new Menu { Id = 7, Name = "Roles", Route = "/admin/masters/roles", ParentId = 2, Sequence = 4, CreatedBy = "Admin" },
            new Menu { Id = 8, Name = "Menus", Route = "/admin/masters/menus", ParentId = 2, Sequence = 5, CreatedBy = "Admin" },
            //Users Menu
            new Menu { Id = 11, Name = "Users", Route = "/admin/user-management/all-users", ParentId = 3, Sequence = 1, CreatedBy = "Admin" },
            new Menu { Id = 12, Name = "Approve Users", Route = "/admin/user-management/approve-user", ParentId = 3, Sequence = 2, CreatedBy = "Admin" },
            new Menu { Id = 13, Name = "Unlock Users", Route = "/admin/user-management/unlock-user", ParentId = 3, Sequence = 3, CreatedBy = "Admin" },
            new Menu { Id = 14, Name = "Assign Roles", Route = "/admin/user-management/assign-roles", ParentId = 3, Sequence = 4, CreatedBy = "Admin" }
        );
    }
}