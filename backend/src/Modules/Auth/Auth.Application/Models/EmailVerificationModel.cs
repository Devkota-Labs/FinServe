namespace Auth.Application.Models;

internal sealed record EmailVerificationModel(string UserName, Uri VerificationLink, int ExpiryTimeInHours);