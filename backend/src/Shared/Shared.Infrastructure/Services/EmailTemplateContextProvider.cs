using Microsoft.Extensions.Options;
using Shared.Infrastructure.Options;

namespace Shared.Infrastructure.Services;

internal sealed class EmailTemplateContextProvider(IOptions<BrandingOptions> appConfig) : IEmailTemplateContextProvider
{
    private readonly BrandingOptions _appConfig = appConfig.Value;
    public EmailTemplateCommonContext GetCommonContext()
    {
        return new EmailTemplateCommonContext
        {
            AppName = _appConfig.AppName,
            CompanyName = _appConfig.CompanyName,
            SupportEmail = _appConfig.SupportEmail,
            AppUrl = _appConfig.AppUrl,
            LogoUrl = _appConfig.LogoUrl,
            Year = DateTime.UtcNow.Year
        };
    }
}
