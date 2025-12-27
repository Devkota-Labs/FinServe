using Auth.Application.Interfaces.Services;
using Auth.Application.Options;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Common.Services;

namespace Auth.Application.Services;

internal sealed class UserNamePolicyService : BaseService, IUserNamePolicyService
{
    private readonly HashSet<string> _reserved;
    public UserNamePolicyService(ILogger logger, IOptions<ReservedUsernameOptions> options)
        : base(logger.ForContext<UserNamePolicyService>(), null)
    {
        _reserved = [.. options.Value.Usernames.Select(Normalize)];
    }

    public bool IsReserved(string userName)
        => _reserved.Contains(Normalize(userName));

    public string Normalize(string userName)
        => userName
            .Trim()
            .ToUpperInvariant()
            .Replace(" ", "", StringComparison.InvariantCultureIgnoreCase);
}