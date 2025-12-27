namespace Auth.Application.Dtos;

public sealed record VerifyResetPasswordDto(string Email, string Code);
