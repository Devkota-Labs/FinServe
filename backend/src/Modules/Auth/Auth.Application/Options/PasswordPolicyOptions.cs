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
    public bool RequireSpecial { get; init; } = true;
    public bool NoWhitespace { get; init; } = true;
    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), MinLength, RequireUppercase, RequireLowercase, RequireDigit, RequireSpecial, NoWhitespace);
    }
}
