using Shared.Common.Configurations;
using Shared.Common.Utils;
using System.Runtime;

namespace FinServe.Api.Configurations;

internal sealed class AppConfig : BaseServiceConfig
{
    public const string SectionName = "AppConfig";
    public GCLatencyMode GCLatencyMode { get; set; } = GCLatencyMode.SustainedLowLatency;

    public override string ToString()
    {
        return Methods.GetToString(base.ToString(), GCLatencyMode);
    }
}
