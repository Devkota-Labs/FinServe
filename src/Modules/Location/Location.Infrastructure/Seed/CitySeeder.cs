using Location.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Location.Infrastructure.Seed;

internal static class CitySeeder
{
    public static void Seed(EntityTypeBuilder<City> builder)
    {
        var seedTime = new DateTime(2024, 1, 1);

        builder.HasData(
            new City { Id = 1, Name = "Mumbai", StateId = 1, CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
            new City { Id = 2, Name = "Pune", StateId = 1, CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
            new City { Id = 3, Name = "Bengaluru", StateId = 2, CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" }
        // NOTE: Full major city list normally inserted here.
        );
    }
}
