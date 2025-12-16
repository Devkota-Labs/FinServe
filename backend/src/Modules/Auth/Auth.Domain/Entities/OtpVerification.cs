using Shared.Domain.Entities;

namespace Auth.Domain.Entities;

public sealed class OtpVerification : BaseEntity
{
    public required int UserId { get; set; }
    public required string Token { get; set; }
    public required OtpPurpose Purpose { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public DateTime? ConsumedAt { get; set; }

    public bool IsConsumed => ConsumedAt.HasValue;
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsConsumed && !IsExpired;
}