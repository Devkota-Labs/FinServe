namespace Auth.Application.Interfaces.Services;

public interface IUserNamePolicyService
{
    bool IsReserved(string userName);
    string Normalize(string userName);
}
