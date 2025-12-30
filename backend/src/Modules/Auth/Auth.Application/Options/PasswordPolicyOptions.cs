using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Auth.Application.Options;

public sealed class PasswordPolicyOptions : BaseServiceConfig
{
    public const string SectionName = "PasswordPolicy";
    public int MinLength { get; init; } = 8;
    public bool RequireUppercase { get; init; } = true;
    public bool RequireLowercase { get; init; } = true;
    public bool RequireDigit { get; init; } = true;
    public bool RequireSpecialCharacter { get; init; } = true;
    public string? SpecialCharacters { get; init; }
    public bool AllowedWhitespace { get; init; }
    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), MinLength, RequireUppercase, RequireLowercase, RequireDigit, RequireSpecialCharacter, SpecialCharacters, AllowedWhitespace);
    }
}
