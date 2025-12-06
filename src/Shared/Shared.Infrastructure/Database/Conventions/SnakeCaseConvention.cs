using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Shared.Infrastructure.Database.Conventions;

internal static class SnakeCaseConvention
{
    public static void UseSnakeCaseNames(this ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToSnakeCase(entity.GetTableName()!));

            foreach (var property in entity.GetProperties())
                property.SetColumnName(ToSnakeCase(property.GetColumnName(StoreObjectIdentifier.Table(entity.GetTableName()!, null))!));
        }
    }

    private static string ToSnakeCase(string name)
    {
        return string.Concat(
            name.Select((x, i) =>
                i > 0 && char.IsUpper(x)
                ? "_" + x
                : x.ToString()
            ));
    }
}
