using Serilog;

namespace Shared.Logging;

internal sealed class SerilogProvider(ILogger logger) : ILogProvider
{
    private readonly ILogger _logger = logger ?? Log.Logger;

    public void Information(string message) => _logger.Information(message);
    public void Warning(string message) => _logger.Warning(message);
    public void Error(string message, Exception? ex = null) => _logger.Error(ex, message);
    public void Debug(string message) => _logger.Debug(message);
}
