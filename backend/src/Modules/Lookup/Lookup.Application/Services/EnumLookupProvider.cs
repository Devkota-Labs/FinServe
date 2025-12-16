using Lookup.Application.Dtos;
using Lookup.Application.Interfaces.Services;
using Shared.Application.Attributes;
using Shared.Application.Results;
using System.Collections.Concurrent;
using System.Reflection;

namespace Lookup.Application.Services;

public sealed class EnumLookupProvider : ILookupProvider
{
    private static readonly ConcurrentDictionary<Type, Result<IReadOnlyList<LookupItemDto>>> Cache = new();

    public Result<IReadOnlyList<LookupItemDto>> Get<TEnum>()
        where TEnum : Enum
    {
        return Cache.GetOrAdd(typeof(TEnum), _ =>
        {
            return Result<IReadOnlyList<LookupItemDto>>.Ok(Enum.GetValues(typeof(TEnum))
                .Cast<Enum>()
                .Select(e =>
                {
                    var field = e.GetType().GetField(e.ToString())!;
                    var attr = field.GetCustomAttribute<LookupCodeAttribute>();

                    if (attr is null)
                        return null;

                    return new LookupItemDto(
                        Convert.ToInt32(e),
                        attr.Code,
                        attr.Label
                    );
                })
                .Where(x => x != null)
                .ToList()!);
        });
    }
}