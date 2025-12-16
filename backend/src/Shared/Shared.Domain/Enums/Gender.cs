using Shared.Application.Attributes;

namespace Shared.Domain.Enums;

public enum Gender : int
{
    Unknown = 0,
    [LookupCode("M", "Male")]
    Male,
    [LookupCode("F", "Female")]
    Female,
    [LookupCode("O", "Other")]
    Other,
    [LookupCode("N", "Perfer Not To Say")]
    PerferNotToSay
};
