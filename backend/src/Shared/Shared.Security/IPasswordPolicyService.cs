using Shared.Application.Dtos;
using Shared.Application.Results;

namespace Shared.Security;

public interface IPasswordPolicyService
{
    (bool IsValid, string Message) ValidatePassword(string password);

    //PasswordPolicy
    Result<PasswordPolicyDto> GetPasswordPolicy();
}
