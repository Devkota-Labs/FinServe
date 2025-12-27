using Shared.Common.Utils;
using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace Users.Domain.Entities;

public sealed class User : BaseAuditableEntity
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Mobile { get; set; }
    public required Gender Gender { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public string FullName => $"{FirstName} {(string.IsNullOrEmpty(MiddleName) ? "" : MiddleName + " ")}{LastName}";
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public Uri? ProfileImageUrl { get; set; }
    public ICollection<UserRole> UserRoles { get; } = [];
    public bool IsActive { get; set; } = true;
    public bool IsApproved { get; set; }
    public bool EmailVerified { get; set; }
    public bool MobileVerified { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime PasswordLastChanged { get; set; } = DateTimeUtil.Now;
    public DateTime? PasswordExpiryDate { get; set; }
    public int FailedLoginCount { get; set; }
    public DateTime? LockoutEndAt { get; set; }
    public bool MfaEnabled { get; set; }
    public string? MfaSecret { get; set; }
    public string? DeviceTokensJson { get; set; }
    private readonly List<UserAddress> _addresses = [];
    public IReadOnlyCollection<UserAddress> Addresses => _addresses;

    public void AddAddress(UserAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (_addresses.Any(a => a.IsPrimary))
            address.UnsetPrimary();

        _addresses.Add(address);
    }
}