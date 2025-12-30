using Auth.Application.Dtos;
using Auth.Application.Interfaces.Services;
using Auth.Application.Options;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;
using Shared.Common.Utils;
using System.Text.RegularExpressions;

namespace Auth.Application.Services;

internal sealed class UserNamePolicyService : BaseService, IUserNamePolicyService
{
    private readonly UserNamePolicyOptions _options;
    private readonly Regex _regex;

    public UserNamePolicyService(ILogger logger, IOptions<UserNamePolicyOptions> options)
        : base(logger.ForContext<UserNamePolicyService>(), null)
    {
        _options = options.Value;
        _regex = new Regex(_options.AllowedPattern, RegexOptions.Compiled);
    }

    public Result<UserNamePolicyDto> GetUserNamePolicy()
    {
        return Result.Ok(new UserNamePolicyDto(_options.MinLength, _options.MaxLength, _options.MustStartWithLetter, _options.CaseSensitive)
        {
            AllowedPattern = _options.AllowedPattern,
        });
    }

    public bool IsReserved(string userName)
    {
        var normalized = _options.CaseSensitive
            ? userName
            : Normalize(userName);

        return _options.ReservedUsernames
            .Any(r => r.Equals(normalized, StringComparison.OrdinalIgnoreCase));
    }

    public string Normalize(string userName)
        => userName
            .Trim()
            .ToUpperInvariant()
            .Replace(" ", "", StringComparison.InvariantCultureIgnoreCase);

    public (bool IsValid, string Message) ValidateUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return (false, "UserName cannot be empty.");

        var normalized = _options.CaseSensitive
            ? userName
            : Normalize(userName);

        if (normalized.Length < _options.MinLength ||
            normalized.Length > _options.MaxLength)
            return (false, $"UserName must be at least {_options.MinLength} and maximum {_options.MaxLength} characters long.");

        if (!_regex.IsMatch(normalized))
            return (false, $"UserName is not as per allowed pattern {_options.AllowedPattern}.");

        if (!IsReserved(userName))
            return (false, $"UserName is not allowed for reserved usernames {Methods.ListToString(_options.ReservedUsernames)}.");

        return (true, "UserName meets complexity requirements.");
    }
}