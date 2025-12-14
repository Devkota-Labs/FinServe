namespace Shared.Application.Interfaces;

public interface ISmsSender
{
    Task SendSmsAsync(string mobileNo, string message, CancellationToken cancellationToken = default);
}
