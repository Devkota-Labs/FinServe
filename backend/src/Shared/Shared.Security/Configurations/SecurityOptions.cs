using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Shared.Security.Configurations;

public sealed class SecurityOptions : BaseServiceConfig
{
    public const string SectionName = "Security";

    public int PasswordExpiryDays { get; init; } = 90;
    public int PasswordHistoryCount { get; init; } = 5;
    public int PasswordExpiryReminderDays { get; init; } = 7;
    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), PasswordExpiryDays, PasswordHistoryCount, PasswordExpiryReminderDays);
    }
}
