using Lookup.Application.Dtos;
using Lookup.Application.Interfaces.Services;
using Shared.Application.Results;
using Shared.Domain.Enums;

namespace Lookup.Application.Lookups;

public class GenderLookup(ILookupProvider provider)
{
    private static readonly Dictionary<Gender, (string Code, string Label)> Map =
        new()
        {
            { Gender.Male, ("M", "Male") },
            { Gender.Female, ("F", "Female") },
            { Gender.Other, ("O", "Other") }
        };

    public static bool TryFromCode(string code, out Gender gender)
    {
        var match = Map.FirstOrDefault(x => x.Value.Code == code);
        gender = match.Key;
        return match.Key != default;
    }

    public static (string Code, string Label) ToDto(Gender gender)
        => Map[gender];

    public Result Get()
        => provider.Get<Gender>();
}
