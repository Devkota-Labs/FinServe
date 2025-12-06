using Shared.Kernel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
using System.Reflection;

namespace Auth.Domain.Entities
{
    public sealed class Country : BaseEntity
    {
        public required string Name { get; set; }
        public required string IsoCode { get; set; }
        public required string MobileCode { get; set; }

        public ICollection<State> States { get; set; }
    }

    public sealed class City : BaseEntity
    {
        public required string Name { get; set; }
        public required int StateId { get; set; }
        public State State { get; set; } = null!;
    }


    public sealed class State : BaseEntity
    {
        public required string Name { get; set; }
        public required int CountryId { get; set; }
        public Country Country { get; set; } = null!;
        public ICollection<City> Cities { get; set; }
    }

    public sealed class PasswordResetToken : BaseEntity
    {
        public required int UserId { get; set; }
        public required string Token { get; set; }
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(30);
        public required bool Used { get; set; }
        public User? User { get; set; }
    }

    public sealed class EmailVerificationToken : BaseEntity
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required DateTime ExpiryTime { get; set; }
        public required bool IsUsed { get; set; }
    }




    public sealed class LoginHistory : BaseEntity
    {
        public int? UserId { get; set; }
        public string Email { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string IpAddress { get; set; }
        public string Device { get; set; }
        public Status Status { get; set; } = Status.SUCCESS;
        public string Message { get; set; }
    }
    public sealed class MenuMaster : BaseEntity
    {
        public required string Name { get; set; }
        public string? Route { get; set; }
        public string? Icon { get; set; }
        public int? ParentId { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;
        public MenuMaster? Parent { get; set; }
        public ICollection<MenuMaster> Children { get; set; }
        public ICollection<RoleMenu> RoleMenus { get; set; }
    }


    public sealed class PasswordHistory : BaseEntity
    {
        public int UserId { get; set; }
        public required string PasswordHash { get; set; }
        public User User { get; set; }
    }

    public sealed class MobileVerificationToken : BaseEntity
    {
        public required string MobileNumber { get; set; }
        public required string Token { get; set; }
        public required DateTime ExpiryTime { get; set; }
        public required bool IsUsed { get; set; }
    }

    public sealed class RefreshToken : BaseEntity
    {
        public required int UserId { get; set; }
        public required string Token { get; set; } = string.Empty;
        public required DateTime ExpiresAt { get; set; }
        public string? CreatedByIp { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }
        public string? ReasonRevoked { get; set; }
        public User? User { get; set; }
    }

    public sealed class RoleMenu : BaseEntity
    {
        public required int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public required int MenuId { get; set; }
        public MenuMaster MenuMaster { get; set; } = null!;
    }

    public sealed class UserRole : BaseEntity
    {
        public required int UserId { get; set; }
        public User User { get; set; } = null!;
        public required int RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }

    public sealed class User : BaseEntity
    {
        public required string Email { get; set; }
        public required string Mobile { get; set; }
        public required Gender Gender { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public string FullName => $"{FirstName} {(string.IsNullOrEmpty(MiddleName) ? "" : MiddleName + " ")}{LastName}";
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public required int CountryId { get; set; }
        public required int CityId { get; set; }
        public required int StateId { get; set; }
        public required string Address { get; set; }
        public required string PinCode { get; set; }
        public Country? Country { get; set; }
        public State? State { get; set; }
        public City? City { get; set; }
        public string? ProfileImageUrl { get; set; }
        // FIXED: EF Core navigation property must be ICollection<>, not List<>
        public ICollection<UserRole> UserRoles { get; set; } = [];
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; }
        public bool EmailVerified { get; set; }
        public bool MobileVerified { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime PasswordLastChanged { get; set; } = DateTime.UtcNow;
        public DateTime? PasswordExpiryDate { get; set; }
        public int FailedLoginCount { get; set; }
        public DateTime? LockoutEndAt { get; set; }
        public bool MfaEnabled { get; set; }
        public string? MfaSecret { get; set; }
        public string? DeviceTokensJson { get; set; }
    }
}
