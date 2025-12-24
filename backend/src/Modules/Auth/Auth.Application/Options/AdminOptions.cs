using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Auth.Application.Options;

internal sealed class AdminOptions : BaseServiceConfig
{
    public const string SectionName = "Admin";
    public string NotificationEmail { get; init; } = string.Empty;
    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), NotificationEmail);
    }
}
