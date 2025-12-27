using Shared.Common.Configurations;
using Shared.Common.Utils;

namespace Shared.Infrastructure.Options;

public sealed class FrontendOptions : BaseServiceConfig
{
    public const string SectionName = "Frontend";
    public Uri BaseUrl { get; init; } = default!;

    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), BaseUrl);
    }
}