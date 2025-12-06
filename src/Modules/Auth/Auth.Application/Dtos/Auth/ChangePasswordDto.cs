namespace Auth.Application.Dtos.Auth;

public sealed record ChangePasswordDto(string OldPassword, string NewPassword);
