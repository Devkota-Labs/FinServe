namespace Auth.Application.Dtos;

public sealed record LoginResponseDto(LoginResponseUserDto User)
{
    public string? AccessToken { get; set; } = null!;
    public string? RefreshToken { get; set; } = null!;
}