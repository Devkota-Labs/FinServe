namespace Users.Application.Dtos.User;

public sealed record UserProfileDto(int Id, string Email, string FirstName, string? MiddleName, string LastName, string Mobile, string Address, Uri? ProfileImageUrl, int? CountryId, int? StateId, int? CityId, string RoleName, DateTime CreatedAt, DateTime? UpdatedAt);
