namespace Shared.Logging;

public interface ILogProvider
{
    void Information(string message);
    void Warning(string message);
    void Error(string message, Exception? ex = null);
    void Debug(string message);
}
