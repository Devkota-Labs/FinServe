namespace Auth.Application.Dtos;

public sealed record LoginDto(string Email, string Password, string? TotpCode);
