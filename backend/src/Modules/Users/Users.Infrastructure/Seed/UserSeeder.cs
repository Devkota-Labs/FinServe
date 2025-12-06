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
            PasswordHash = "AMjWYtPmGZURetbp6dI1r3XfmgZy0nrn7FU7te333XPDv3gqwQzRZNtcoka4Sow++Q==",
            PasswordLastChanged = seedTime,
            CreatedTime = seedTime,
            CreatedBy = "Admin"
        };

        builder.HasData(adminUser);
    }
}
