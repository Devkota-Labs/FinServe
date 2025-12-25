namespace Auth.Application.Models;

internal sealed record PasswordResetModel(string UserName, Uri ResetLink, int ExpiryTimeInMinutes);

