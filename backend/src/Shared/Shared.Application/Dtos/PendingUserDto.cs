namespace Shared.Application.Dtos;

public sealed record PendingUserDto(int Id, string UserName, string FullName, string Email, string MobileNo, bool IsApproved, bool IsEmailVerified, bool IsMobileVerified, bool IsActive, DateTime CreatedAt);