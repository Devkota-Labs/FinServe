using Auth.Application.Options;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace Auth.Infrastructure.Services;

internal sealed class PasswordPolicyService(IOptions<PasswordPolicyOptions> passwordPolicyOptions)
    : Shared.Security.IPasswordPolicyService
{
    private readonly PasswordPolicyOptions _passwordPolicyOptions = passwordPolicyOptions.Value;

    public (bool IsValid, string Message) ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return (false, "Password cannot be empty.");

        if (password.Length < _passwordPolicyOptions.MinLength)
            return (false, $"Password must be at least {_passwordPolicyOptions.MinLength} characters long.");

        if (_passwordPolicyOptions.RequireUppercase && !Regex.IsMatch(password, @"[A-Z]"))
            return (false, "Password must contain at least one uppercase letter.");

        if (_passwordPolicyOptions.RequireLowercase && !Regex.IsMatch(password, @"[a-z]"))
            return (false, "Password must contain at least one lowercase letter.");

        if (_passwordPolicyOptions.RequireDigit && !Regex.IsMatch(password, @"[0-9]"))
            return (false, "Password must contain at least one digit.");

        if (_passwordPolicyOptions.RequireSpecial && !Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>_\-+=]"))
            return (false, "Password must contain at least one special character.");

        if (_passwordPolicyOptions.NoWhitespace && Regex.IsMatch(password, @"\s"))
            return (false, "Password must not contain spaces.");

        return (true, "Password meets complexity requirements.");
    }
}