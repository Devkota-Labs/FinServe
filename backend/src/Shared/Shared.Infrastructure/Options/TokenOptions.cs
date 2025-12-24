using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Shared.Infrastructure.Options;

public sealed class TokenOptions : BaseServiceConfig
{
    public const string SectionName = "Token";
    public int EmailVerificationExpiryMinutes { get; init; } = 1440; // 24h
    public int PasswordResetExpiryMinutes { get; init; } = 30;

    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), EmailVerificationExpiryMinutes, PasswordResetExpiryMinutes);
    }
}
