using Shared.Domain;

namespace Users.Domain.Entities;

public sealed class UserRole : BaseAuditableEntity
{
    public required int UserId { get; set; }
    public required int RoleId { get; set; }
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}