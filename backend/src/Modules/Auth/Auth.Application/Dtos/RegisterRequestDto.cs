namespace Auth.Application.Dtos;

public sealed record RegisterRequestDto(string UserName, string Email, string Mobile, string Gender, DateOnly DateOfBirth, string FirstName, string? MiddleName, string LastName, int CountryId, int CityId, int StateId, string Address, string PinCode, string Password);
