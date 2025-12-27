using Shared.Common.Utils;
using Shared.Domain.Entities;

namespace Auth.Domain.Entities;

public sealed class RefreshToken : BaseEntity
{
    public required int UserId { get; set; }
    public required string Token { get; set; } = string.Empty;
    public required bool IsUsed { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public string? CreatedByIp { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }

    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsExpired => DateTimeUtil.Now >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
}
