using Shared.Application.Results;
using Users.Application.Dtos.User;

namespace Auth.Application.Interfaces.Services;

public interface IUserNameAvailabilityService
{
    Task<Result<UserNameAvailabilityResponseDto>> IsAvailableAsync(string userName, int maxSuggestions, CancellationToken cancellationToken = default);
}
