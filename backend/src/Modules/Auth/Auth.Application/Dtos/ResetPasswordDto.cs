namespace Auth.Application.Dtos;

public sealed record ResetPasswordDto(string Email, string Token, string NewPassword);
