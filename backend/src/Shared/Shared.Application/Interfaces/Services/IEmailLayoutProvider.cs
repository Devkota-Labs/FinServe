namespace Shared.Application.Interfaces.Services;

public interface IEmailLayoutProvider
{
    string LoadLayout(string layoutName);
}
