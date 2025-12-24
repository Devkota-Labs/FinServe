using Users.Application.Dtos.User;

namespace Auth.Application.Dtos;

public sealed record RegisterRequestDto(string UserName, string Email, string Mobile, string Gender, DateOnly DateOfBirth, string FirstName, string? MiddleName, string LastName, ICollection<CreateAddressDto> Addresses, string Password);
