namespace Auth.Application.Dtos;

public sealed record ChangePasswordDto(string OldPassword, string NewPassword);
