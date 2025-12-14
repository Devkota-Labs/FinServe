using Shared.Domain;

namespace Auth.Domain.Entities;

public sealed class MobileVerificationToken : BaseEntity
{
    public required int UserId { get; set; }
    public required string MobileNumber { get; set; }
    public required string Token { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public required bool IsUsed { get; set; }
}