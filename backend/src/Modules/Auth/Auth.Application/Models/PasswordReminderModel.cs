namespace Auth.Application.Models;

internal sealed record PasswordReminderModel(string UserName, int DaysLeft, Uri ChangePasswordUrl);

