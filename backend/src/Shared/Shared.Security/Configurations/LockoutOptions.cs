using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Shared.Security.Configurations;

public sealed class LockoutOptions : BaseServiceConfig
{
    public const string SectionName = "Lockout";
    public int MaxFailedAttempts { get; init; } = 5;
    public int LockoutMinutes { get; init; } = 15;
    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), MaxFailedAttempts, LockoutMinutes);
    }
}
