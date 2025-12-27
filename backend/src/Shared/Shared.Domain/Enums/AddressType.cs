using Shared.Application.Attributes;

namespace Shared.Domain.Enums;

public enum AddressType : int
{
    Unknown = 0,
    [LookupCode("H", "Home")]
    Home,
    [LookupCode("W", "Work")]
    Work,
    [LookupCode("O", "Other")]
    Other,
}