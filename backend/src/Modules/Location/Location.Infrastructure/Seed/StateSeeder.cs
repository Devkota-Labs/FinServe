using Location.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Location.Infrastructure.Seed;

internal static class StateSeeder
{
    public static void Seed(EntityTypeBuilder<State> builder)
    {
        var seedTime = new DateTime(2024, 1, 1);

        builder.HasData(
            new State { Id = 1, Name = "Maharashtra", CountryId = 75, CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
            new State { Id = 2, Name = "Karnataka", CountryId = 75, CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" },
            new State { Id = 3, Name = "Tamil Nadu", CountryId = 75, CreatedTime = seedTime, LastUpdatedTime = seedTime, CreatedBy = "Seeder", LastUpdatedBy = "Seeder" }
        // NOTE: Full Indian state + UT list normally inserted here.
        );
    }
}
