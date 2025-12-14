namespace Users.Application.Dtos.User;

public sealed record UpdateUserDto(string FirstName, string? MiddleName, string LastName, string Address, Uri? ProfileImageUrl, int? CountryId, int? StateId, int? CityId);
