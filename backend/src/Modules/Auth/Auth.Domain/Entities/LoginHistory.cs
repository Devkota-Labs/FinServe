using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace Auth.Domain.Entities;

public sealed class LoginHistory : BaseEntity
{
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime? LogoutTime { get; set; }
    public bool IsSuccess { get; set; }
    public string? FailureReason { get; set; }
    public string IpAddress { get; set; } = null!;
    public string UserAgent { get; set; } = default!;
    public string Device { get; set; } = null!;
    public Status Status { get; set; } = Status.SUCCESS;
}