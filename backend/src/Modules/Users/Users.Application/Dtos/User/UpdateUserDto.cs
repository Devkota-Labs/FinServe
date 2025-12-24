namespace Users.Application.Dtos.User;

public sealed record UpdateUserDto(string FirstName, string? MiddleName, string LastName, Uri? ProfileImageUrl);
