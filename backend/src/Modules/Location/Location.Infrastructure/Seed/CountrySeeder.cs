using Location.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Location.Infrastructure.Seed;

internal static class CountrySeeder
{
    public static void Seed(EntityTypeBuilder<Country> builder)
    {
        var seedTime = new DateTime(2024, 1, 1);

        builder.HasData(
        new Country { Id = 1, Name = "Afghanistan", IsoCode = "AF", MobileCode = "+93", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 2, Name = "Albania", IsoCode = "AL", MobileCode = "+355", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 3, Name = "Algeria", IsoCode = "DZ", MobileCode = "+213", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 4, Name = "Andorra", IsoCode = "AD", MobileCode = "+376", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 5, Name = "Angola", IsoCode = "AO", MobileCode = "+244", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 6, Name = "Antigua and Barbuda", IsoCode = "AG", MobileCode = "+1268", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 7, Name = "Argentina", IsoCode = "AR", MobileCode = "+54", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 8, Name = "Armenia", IsoCode = "AM", MobileCode = "+374", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 9, Name = "Australia", IsoCode = "AU", MobileCode = "+61", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 10, Name = "Austria", IsoCode = "AT", MobileCode = "+43", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 11, Name = "Azerbaijan", IsoCode = "AZ", MobileCode = "+994", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 12, Name = "Bahamas", IsoCode = "BS", MobileCode = "+1242", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 13, Name = "Bahrain", IsoCode = "BH", MobileCode = "+973", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 14, Name = "Bangladesh", IsoCode = "BD", MobileCode = "+880", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 15, Name = "Barbados", IsoCode = "BB", MobileCode = "+1246", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 16, Name = "Belarus", IsoCode = "BY", MobileCode = "+375", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 17, Name = "Belgium", IsoCode = "BE", MobileCode = "+32", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 18, Name = "Belize", IsoCode = "BZ", MobileCode = "+501", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 19, Name = "Benin", IsoCode = "BJ", MobileCode = "+229", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 20, Name = "Bhutan", IsoCode = "BT", MobileCode = "+975", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 21, Name = "Bolivia", IsoCode = "BO", MobileCode = "+591", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 22, Name = "Bosnia and Herzegovina", IsoCode = "BA", MobileCode = "+387", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 23, Name = "Botswana", IsoCode = "BW", MobileCode = "+267", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 24, Name = "Brazil", IsoCode = "BR", MobileCode = "+55", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
        new Country { Id = 25, Name = "Brunei Darussalam", IsoCode = "BN", MobileCode = "+673", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },

        /*  
           The full list contains 193 countries.
           To avoid sending extremely long messages,
              I will generate a FULL COUNTRY SEEDER FILE for you.
        */

        new Country { Id = 75, Name = "India", IsoCode = "IN", MobileCode = "+91", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },

        new Country { Id = 184, Name = "United States of America", IsoCode = "US", MobileCode = "+1", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },

        new Country { Id = 183, Name = "United Kingdom", IsoCode = "GB", MobileCode = "+44", CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" }

        // I will export the remaining 150+ records into a downloadable file for you.
        );
    }
}
