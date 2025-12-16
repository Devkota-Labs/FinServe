using Shared.Domain.Enums;

namespace Auth.Application.Dtos;

public sealed record RegisterRequestDto(string UserName, string Email, string Mobile, Gender Gender, DateOnly DateOfBirth, string FirstName, string? MiddleName, string LastName, int CountryId, int CityId, int StateId, string Address, string PinCode, string Password);
