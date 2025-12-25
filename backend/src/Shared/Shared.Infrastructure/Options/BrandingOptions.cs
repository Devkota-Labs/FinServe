using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Shared.Infrastructure.Options;

public sealed class BrandingOptions : BaseServiceConfig
{
    public const string SectionName = "Branding";
    public string AppName { get; init; } = default!;
    public string CompanyName { get; init; } = default!;

    // Visual identity
    public Uri LogoUrl { get; init; } = default!;
    public string PrimaryColor { get; init; } = default!;
    public string SecondaryColor { get; init; } = default!;

    // Contact & identity
    public string SupportEmail { get; init; } = default!;
    public string SupportPhone { get; init; } = default!;

    // Legal & footer
    public string CompanyAddress { get; init; } = default!;
    public string CopyrightText { get; init; } = default!;

    // URLs shown to users
    public Uri WebsiteUrl { get; init; } = default!;
    public Uri AppUrl { get; init; } = default!;
    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), AppName, CompanyName, LogoUrl, PrimaryColor, SecondaryColor, SupportEmail, SupportPhone, CompanyAddress, CopyrightText, WebsiteUrl, AppUrl);
    }
}
