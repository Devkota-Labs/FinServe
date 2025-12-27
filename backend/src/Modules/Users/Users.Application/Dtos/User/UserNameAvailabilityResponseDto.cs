namespace Users.Application.Dtos.User;

public sealed record UserNameAvailabilityResponseDto(bool IsAvailable, string Message, IReadOnlyCollection<string>? Suggesstions);
