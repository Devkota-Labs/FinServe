namespace Users.Application.Dtos.User;

public sealed record UserProfileDto(int Id, string Email, string FirstName, string? MiddleName, string LastName, string Mobile, Uri? ProfileImageUrl, IEnumerable<AddressDto> Addresses, DateTime CreatedAt, DateTime? UpdatedAt);
