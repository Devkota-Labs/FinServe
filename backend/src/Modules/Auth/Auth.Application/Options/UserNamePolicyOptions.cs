using Shared.Common.Configurations;

namespace Auth.Application.Options;

public sealed class UserNamePolicyOptions : BaseServiceConfig
{
    public const string SectionName = "UserNamePolicy";
    public int MinLength { get; init; }
    public int MaxLength { get; init; }
    public string AllowedPattern { get; init; } = default!;
    public bool MustStartWithLetter { get; init; }
    public bool CaseSensitive { get; init; }
    public IReadOnlyList<string> ReservedUsernames { get; init; } = [];
}