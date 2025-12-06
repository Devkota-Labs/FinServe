using Shared.Domain;

namespace Users.Domain.Entities;

public sealed class RoleMenu : BaseAuditableEntity
{
    public required int RoleId { get; set; }
    public required int MenuId { get; set; }
    public Role Role { get; set; } = null!;
    public Menu Menu { get; set; } = null!;
}