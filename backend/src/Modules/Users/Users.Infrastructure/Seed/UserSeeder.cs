using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Enums;
using Users.Domain.Entities;

namespace Users.Infrastructure.Seed;

internal static class UserSeeder
{
    public static void Seed(EntityTypeBuilder<User> builder)
    {
        var seedTime = new DateTime(2024, 1, 1);

        var adminUser = new User
        {
            Id = 1,
            UserName = "Admin",
            Email = "admin@finserve.com",
            Mobile = "9999999999",
            Gender = Gender.PerferNotToSay,
            DateOfBirth = new DateOnly(seedTime.Year, seedTime.Month, seedTime.Day),
            FirstName = "System",
            LastName = "Administrator",
            CountryId = 1,
            StateId = 1,
            CityId = 1,
            Address = "123 Admin St, Metropolis",
            PinCode = "400001",
            //UserRoles = 1,
            IsActive = true,
            IsApproved = true,
            EmailVerified = true,
            MobileVerified = true,
            // Hash Password
            PasswordHash = "AENIe4W4SbJh10PQDlXCrz7vyJmLQulPRIuFhXqE+p41Pf0DRGhLa+CDx6EkNjHfhg==",
            PasswordLastChanged = seedTime,
            CreatedTime = seedTime,
            CreatedBy = "Admin"
        };

        builder.HasData(adminUser);
    }
}
