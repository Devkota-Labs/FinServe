using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Shared.Infrastructure.Options;

public sealed class EmailOptions : BaseServiceConfig
{
    public const string SectionName = "Email";
    // Transport
    public string SmtpHost { get; init; } = default!;
    public int SmtpPort { get; init; }
    public string SmtpUser { get; init; } = default!;
    public string SmtpPassword { get; init; } = default!;
    public bool UseSsl { get; init; }
    // Sender identity
    public string FromEmail { get; init; } = default!;
    public string FromName { get; init; } = default!;

    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), SmtpHost, SmtpPort, SmtpUser, SmtpPassword, UseSsl, FromEmail, FromName);
    }
}
