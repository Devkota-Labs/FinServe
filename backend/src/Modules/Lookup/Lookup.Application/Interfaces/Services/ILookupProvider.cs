using Lookup.Application.Dtos;
using Shared.Application.Results;

namespace Lookup.Application.Interfaces.Services;

public interface ILookupProvider
{
    Result<IReadOnlyList<LookupItemDto>> Get<TEnum>() where TEnum : Enum;
}