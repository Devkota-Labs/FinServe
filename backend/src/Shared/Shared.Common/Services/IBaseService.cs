using Serilog;

namespace Shared.Common.Services;

public interface IBaseService
{
    public ILogger Logger { get; }

    public string Name { get; set; }
}
