using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Database;
using Users.Domain.Entities;

namespace Users.Infrastructure.Db;

internal sealed class UserDbContext(DbContextOptions<UserDbContext> options) : BaseDbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Menu> Menus { get; set; } = null!;
    public DbSet<RoleMenu> RoleMenus { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var module = "Users";

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);

        // ⭐ Auto-prefix all table names
        ApplyModuleTablePrefix(modelBuilder, module);
    }
}
