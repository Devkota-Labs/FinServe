using Shared.Common.Services;

namespace FinServe.Api.Services;

internal sealed class CustomConsoleLiftime(Serilog.ILogger logger) : BaseService(logger.ForContext<CustomConsoleLiftime>(), null), IHostLifetime, IDisposable
{
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        Console.CancelKeyPress += OnCancelKeyPressed;
        return Task.CompletedTask;
    }
    private void OnCancelKeyPressed(object? sender, ConsoleCancelEventArgs e)
    {
        Logger.Information("Ctrl+C has beed pressed, Ignoring the event.");
        e.Cancel = true;
    }
    public void Dispose()
    {
        Console.CancelKeyPress -= OnCancelKeyPressed;
    }

}
