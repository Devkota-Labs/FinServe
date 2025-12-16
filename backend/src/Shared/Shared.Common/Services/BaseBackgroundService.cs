using Microsoft.Extensions.Hosting;
using Serilog;
using Shared.Common.Configurations;

namespace Shared.Common.Services;

public abstract class BaseBackgroundService : BackgroundService, IBaseService
{
    public ILogger Logger { get; }

    public string Name { get; set; }
    protected BaseBackgroundService(ILogger logger, BaseServiceConfig? baseServiceConfig)
    {
        Logger = logger;
        Name = GetType().Name;

        var id = Guid.NewGuid();

        Logger.Debug("Background Service {ServiceName} with Id {ServiceId} and config {ServiceConfig} has been created.", Name, id, baseServiceConfig);
    }
}
