using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Notification.Application.Options;

public sealed class ScheduledJobsOptions : BaseServiceConfig
{
    public const string SectionName = "ScheduledJobs";
    public int PasswordExpiryCheckHourUtc { get; init; } = -1;
    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), PasswordExpiryCheckHourUtc);
    }
}
