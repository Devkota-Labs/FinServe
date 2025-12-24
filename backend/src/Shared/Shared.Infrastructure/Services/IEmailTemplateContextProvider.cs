using Shared.Infrastructure.Options;

namespace Shared.Infrastructure.Services;

public interface IEmailTemplateContextProvider
{
    EmailTemplateCommonContext GetCommonContext();
}
