namespace Auth.Application.Dtos;

public sealed record LoginResponseDto(LoginResponseUserDto User)
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
