using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Shared.Infrastructure.Options;

public sealed class OtpOptions : BaseServiceConfig
{
    public const string SectionName = "Otp";
    public OtpChannel Channel { get; set; } = OtpChannel.Email;
    public int Length { get; init; } = 6;
    public int ExpiryMinutes { get; init; } = 5;

    public override string ToString()
    {
        return Methods.GetToString( base.ToString(), Channel, Length, ExpiryMinutes);
    }
}
