namespace Users.Application.Dtos.User;

public sealed record UserDto(int Id, string? FirstName, string? MiddleName, string? LastName, string? Mobile, Uri? ProfileImageUrl, IReadOnlyCollection<AddressDto> Addresses);
