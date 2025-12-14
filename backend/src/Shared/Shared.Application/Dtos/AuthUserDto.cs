namespace Shared.Application.Dtos;

public sealed class AuthUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public required string FullName { get; set; }
    public string Email { get; set; } = default!;
    public string MobileNo { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public bool IsEmailVerified { get; set; }
    public bool IsMobileVerified { get; set; }
    public bool IsActive { get; set; }
    public required Uri? ProfileImageUrl { get; set; }
    public required bool IsApproved { get; set; }
    public required DateTime? LockoutEndAt { get; set; }
    public required bool MfaEnabled { get; set; }
    public required string? MfaSecret { get; set; }
    public required DateTime? PasswordExpiryDate { get; set; }
    public ICollection<string>? Roles { get; }
}