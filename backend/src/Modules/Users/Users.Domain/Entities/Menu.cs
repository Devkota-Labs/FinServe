using Shared.Domain;

namespace Users.Domain.Entities;

public sealed class Menu : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? Route { get; set; }
    public string? Icon { get; set; }
    public int? ParentId { get; set; }
    public int Sequence { get; set; }
    public bool IsActive { get; set; } = true;
    public Menu? Parent { get; set; }
    public ICollection<Menu>? Children { get; }
    public ICollection<RoleMenu>? RoleMenus { get; }
}
