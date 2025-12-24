namespace Auth.Application.Models;

internal sealed record OtpEmailModel(string UserName, string Otp, int ExpiryMinutes);

