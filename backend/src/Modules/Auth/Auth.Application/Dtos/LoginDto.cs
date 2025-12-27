namespace Auth.Application.Dtos;

public sealed record LoginDto(string EmailOrUserName, string Password, string? TotpCode);
