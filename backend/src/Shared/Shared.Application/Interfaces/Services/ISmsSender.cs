namespace Shared.Application.Interfaces.Services;

public interface ISmsSender
{
    Task SendSmsAsync(string mobileNo, string message, CancellationToken cancellationToken = default);
}
