using Shared.Domain.Entities;

namespace Auth.Domain.Entities;

public sealed class PasswordHistory : BaseEntity
{
    public int UserId { get; set; }
    public required string PasswordHash { get; set; }
    public required DateTime CreatedTime { get; set; }
}
