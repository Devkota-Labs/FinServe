using Shared.Domain.Entities;

namespace Users.Domain.Entities;

public sealed class Role : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsActive { get; set; } = true;
    public ICollection<UserRole> UserRoles { get; } = [];
    public ICollection<RoleMenu> RoleMenus { get;} = [];
}
