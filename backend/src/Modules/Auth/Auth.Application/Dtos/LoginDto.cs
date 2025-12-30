namespace Auth.Application.Dtos;

public sealed record LoginDto(string Login, string Password, string? TotpCode);
