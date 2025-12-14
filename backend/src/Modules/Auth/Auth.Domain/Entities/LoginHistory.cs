using Shared.Domain;
using Shared.Domain.Enums;

namespace Auth.Domain.Entities;

public sealed class LoginHistory : BaseEntity
{
    public int? UserId { get; set; }
    public string Email { get; set; } = null!;
    public DateTime? LoginTime { get; set; }
    public DateTime? LogoutTime { get; set; }
    public string IpAddress { get; set; } = null!;
    public string Device { get; set; } = null!;
    public Status Status { get; set; } = Status.SUCCESS;
    public string Message { get; set; } = null!;
}