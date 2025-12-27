using Auth.Application.Interfaces.Services;
using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;
using Users.Application.Dtos.User;
using Users.Application.Interfaces.Services;

namespace Auth.Application.Services;

internal sealed class UserNameAvailabilityService(
    ILogger logger,
    IUserReadService userReadService,
    IUserNamePolicyService usernamePolicy
    )
    : BaseService(logger.ForContext<UserNameAvailabilityService>(), null)
    , IUserNameAvailabilityService
{
    public async Task<Result<UserNameAvailabilityResponseDto>> IsAvailableAsync(string userName, int maxSuggestions, CancellationToken cancellationToken = default)
    {
        if (usernamePolicy.IsReserved(userName))
            return Result.Fail<UserNameAvailabilityResponseDto>($"UserName {userName} is not allowed.");

        var isAvailable = await userReadService.IsUserNameAvailableAsync(userName, cancellationToken).ConfigureAwait(false);

        if (isAvailable)
        {
            var suggestions = new List<string>();

            var normalized = usernamePolicy.Normalize(userName);

            var random = new Random();

            while (suggestions.Count < maxSuggestions)
            {
#pragma warning disable CA5394 // Do not use insecure randomness
                var candidate = $"{normalized}{random.Next(100, 9999)}";
#pragma warning restore CA5394 // Do not use insecure randomness

                if (!await userReadService.IsUserNameAvailableAsync(candidate, cancellationToken).ConfigureAwait(false))
                {
                    suggestions.Add(candidate);
                }
            }

            return Result.Ok(new UserNameAvailabilityResponseDto(false, $"Username '{userName}' is not available.", suggestions));
        }
        else
        {
            return Result.Ok(new UserNameAvailabilityResponseDto(true, $"Username '{userName}' is available.", null));
        }
    }
}

