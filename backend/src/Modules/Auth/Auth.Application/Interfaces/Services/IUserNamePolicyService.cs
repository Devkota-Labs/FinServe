using Auth.Application.Dtos;
using Shared.Application.Results;

namespace Auth.Application.Interfaces.Services;

public interface IUserNamePolicyService
{
    public (bool IsValid, string Message) ValidateUserName(string userName);
    bool IsReserved(string userName);
    string Normalize(string userName);
    Result<UserNamePolicyDto> GetUserNamePolicy();
}