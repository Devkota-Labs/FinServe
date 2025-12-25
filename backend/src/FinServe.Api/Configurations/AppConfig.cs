using Shared.Common.Configurations;
using Shared.Common.Utils;
using Shared.Infrastructure.Options;
using System.Runtime;

namespace FinServe.Api.Configurations;

internal sealed class AppConfig : BaseServiceConfig
{
    public const string SectionName = "AppConfig";
    public GCLatencyMode GCLatencyMode { get; set; } = GCLatencyMode.SustainedLowLatency;
    public required BrandingOptions Branding { get; set; }
    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), GCLatencyMode, Branding);
    }
}
