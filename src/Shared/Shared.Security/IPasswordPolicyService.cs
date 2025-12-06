namespace Shared.Security;

public interface IPasswordPolicyService
{
    (bool IsValid, string Message) ValidatePassword(string password);
}
