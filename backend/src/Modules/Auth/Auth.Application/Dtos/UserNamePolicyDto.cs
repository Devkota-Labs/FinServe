namespace Auth.Application.Dtos;

public sealed record UserNamePolicyDto(int MinLength, int MaxLength, bool MustStartWithLetter, bool CaseSensitive)
{
    public string AllowedPattern { get; init; } = default!;
}