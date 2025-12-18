using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Shared.Security.Configurations;

internal sealed class JwtOptions : BaseServiceConfig
{
    public const string SectionName = "AppConfig:Jwt";
    public required string Key { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public int ExpiryMinutes { get; set; } = 60;

    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), Key, Issuer, Audience, ExpiryMinutes);
    }
}
