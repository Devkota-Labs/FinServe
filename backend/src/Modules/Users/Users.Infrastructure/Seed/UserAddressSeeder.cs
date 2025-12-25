using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Enums;
using Users.Domain.Entities;

namespace Users.Infrastructure.Seed;

internal static class UserAddressSeeder
{
    public static void Seed(EntityTypeBuilder<UserAddress> builder)
    {
        var seedTime = new DateTime(2024, 1, 1);

        var userAddress = new UserAddress
        {
            Id = 1,
            AddressType = AddressType.Home,
            UserId = 1,
            AddressLine1 = "123 Admin St",
            CityId = 1,
            StateId = 1,
            CountryId = 1,
            PinCode = "400001",
            IsPrimary = true,
            CreatedTime = seedTime,
            CreatedBy = "Admin"
        };

        builder.HasData(userAddress);
    }
}
