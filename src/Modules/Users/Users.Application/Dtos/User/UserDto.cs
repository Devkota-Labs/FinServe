namespace Users.Application.Dtos.User;

public sealed record UserDto(string? FirstName, string? MiddleName, string? LastName, string? Mobile, string? Address, Uri? ProfileImageUrl, int? CountryId, int? StateId, int? CityId);
