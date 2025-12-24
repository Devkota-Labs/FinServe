using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Auth.Application.Options;

internal sealed class ApiOptions : BaseServiceConfig
{
    public const string SectionName = "Api";
    public string ApprovedUserVerificationVersion { get; init; } = "1";
    public string EmailVerificationVersion { get; init; } = "1";
    public string PasswordResetVerificationVersion { get; init; } = "1";
    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), ApprovedUserVerificationVersion, EmailVerificationVersion, PasswordResetVerificationVersion);
    }
}
