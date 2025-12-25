namespace Notification.Application.Dtos;

public sealed record NotificationDto(int Id, string Title, string Message, string Category, string Severity, string ActionType, int? ReferenceId, bool IsRead, DateTime CreatedAt);