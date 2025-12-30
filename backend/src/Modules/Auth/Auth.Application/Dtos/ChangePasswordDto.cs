namespace Auth.Application.Dtos;

public sealed record ChangePasswordDto(string CurrentPassword, string NewPassword);
