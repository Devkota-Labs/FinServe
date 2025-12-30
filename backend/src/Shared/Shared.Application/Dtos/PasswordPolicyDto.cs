namespace Shared.Application.Dtos;

public sealed record PasswordPolicyDto(int MinLength, bool RequireUppercase, bool RequireLowercase, bool RequireDigit, bool RequireSpecialCharacter, string? SpecialCharacters, bool AllowedWhitespace);