namespace Auth.Application.Dtos;

public sealed record VerifyEmailDto(string Email, string Code);
