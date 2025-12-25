using Shared.Domain.Enums;

namespace Users.Application.Dtos.User;

public sealed record CreateUserDto(string UserName, string Email, string Mobile, Gender Gender, DateOnly DateOfBirth, string FirstName, string? MiddleName, string LastName, ICollection<CreateAddressDto> Address, string Password)
{
    public string FullName => MiddleName is null ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
}
