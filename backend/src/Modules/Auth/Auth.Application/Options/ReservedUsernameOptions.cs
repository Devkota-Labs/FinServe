using Shared.Common.Configurations;

namespace Auth.Application.Options;

public sealed class ReservedUsernameOptions : BaseServiceConfig
{
    public const string SectionName = "ReservedUsernames";
    public HashSet<string> Usernames { get; init; } = [];
}