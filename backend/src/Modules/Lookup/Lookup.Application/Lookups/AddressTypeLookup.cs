using Lookup.Application.Dtos;
using Lookup.Application.Interfaces.Services;
using Shared.Application.Results;
using Shared.Domain.Enums;

namespace Lookup.Application.Lookups;

public class AddressTypeLookup(ILookupProvider provider)
{
    public Result<IReadOnlyList<LookupItemDto>> Get()
        => provider.Get<AddressType>();
}