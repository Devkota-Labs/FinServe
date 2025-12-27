namespace Users.Application.Dtos.User;

public sealed record LockedUserDto(int Id, string FullName, string Email, string Mobile, DateTime LockoutEndAt);
