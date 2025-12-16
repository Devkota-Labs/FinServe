using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Shared.Infrastructure.Options;

public sealed class EmailOptions : BaseServiceConfig
{
    public string SmtpHost { get; init; } = default!;
    public int SmtpPort { get; init; }
    public string UserName { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string FromEmail { get; init; } = default!;
    public string FromName { get; init; } = default!;

    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), SmtpHost, SmtpPort, UserName, Password, FromEmail, FromName);
    }
}
