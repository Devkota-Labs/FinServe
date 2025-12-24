namespace Shared.Infrastructure.Options;

public sealed class EmailTemplateCommonContext
{
    public string AppName { get; init; } = default!;
    public string CompanyName { get; init; } = default!;
    public string SupportEmail { get; init; } = default!;
    public Uri AppUrl { get; init; } = default!;
    public Uri LogoUrl { get; init; } = default!;
    public int Year { get; init; }
}
