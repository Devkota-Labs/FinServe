using Shared.Domain.Enums;

namespace Users.Application.Dtos.User;

public sealed record CreateUserDto(string UserName, string Email, string Mobile, Gender Gender, DateOnly DateOfBirth, string FirstName, string? MiddleName, string LastName, int CountryId, int CityId, int StateId, string Address, string PinCode, string Password)
{
    public string FullName => MiddleName is null ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
}
