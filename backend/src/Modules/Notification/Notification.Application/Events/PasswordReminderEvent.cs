namespace Notification.Application.Events;

public record PasswordReminderEvent(int UserId, DateTime ExpiryDate);
