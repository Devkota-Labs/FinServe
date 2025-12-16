using Microsoft.Extensions.Configuration;
using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;
using Shared.Security;
using Users.Application.Dtos.User;
using Users.Application.Interfaces.Repositories;
using Users.Application.Interfaces.Services;
using Users.Domain.Entities;

namespace Users.Application.Services;

internal sealed class UserService(ILogger logger, IUserRepository repo, IPasswordPolicyService passwordPolicyService, IConfiguration configuration)
: BaseService(logger.ForContext<UserService>(), null), IUserService
{
    public async Task<Result<ICollection<UserDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repo.GetAllAsync(cancellationToken).ConfigureAwait(false);

        var result = entities.Select(Map).ToList();

        return Result<ICollection<UserDto>>.Ok(result);
    }

    public async Task<Result<UserDto?>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entities = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entities == null)
            return Result<UserDto?>.Fail($"User not found with id {id}");

        return Result<UserDto?>.Ok(Map(entities));
    }

    public async Task<Result<UserDto>> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return Result<UserDto>.Fail("Email or password required.");

        var (valid, message) = passwordPolicyService.ValidatePassword(dto.Password);

        if (!valid)
            return Result<UserDto>.Fail(message);

        var emailExists = await repo.GetByEmailAsync(dto.Email, cancellationToken).ConfigureAwait(false);

        if (emailExists != null)
            return Result<UserDto>.Fail("Email already exists.");

        var userNameExists = await repo.GetByNameAsync(dto.UserName, cancellationToken).ConfigureAwait(false);

        if (userNameExists is not null)
        {
            return Result<UserDto>.Fail($"User with name {userNameExists.FullName} already exists.");
        }

        var newEntity = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            Mobile = dto.Mobile,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            CountryId = dto.CountryId,
            CityId = dto.CityId,
            StateId = dto.StateId,
            Address = dto.Address,
            PinCode = dto.PinCode,
            IsActive = true,
            IsApproved = false,
            PasswordHash = dto.Password,
            PasswordLastChanged = DateTime.UtcNow,
            PasswordExpiryDate = DateTime.UtcNow.AddDays(configuration.GetValue("AppConfig:Security:PasswordExpiryDays", 90))
        };

        await repo.AddAsync(newEntity, cancellationToken).ConfigureAwait(false);

        return Result<UserDto>.Ok("User created successfully.", Map(newEntity));
    }

    public async Task<Result<UserDto>> UpdateAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result<UserDto>.Fail("User not found.");

        // Basic info
        if (dto.FirstName is not null) entity.FirstName = dto.FirstName;
        if (dto.MiddleName is not null) entity.MiddleName = dto.MiddleName;
        if (dto.LastName is not null) entity.LastName = dto.LastName;
        if (dto.Address is not null) entity.Address = dto.Address;
        if (dto.ProfileImageUrl is not null) entity.ProfileImageUrl = dto.ProfileImageUrl;

        // Location info
        if (dto.CountryId is not null) entity.CountryId = dto.CountryId.Value;
        if (dto.StateId is not null) entity.StateId = dto.StateId.Value;
        if (dto.CityId is not null) entity.CityId = dto.CityId.Value;

        entity.LastUpdatedTime = DateTime.UtcNow;

        await repo.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result<UserDto>.Ok("User updated successfully", Map(entity));
    }

    public async Task<Result<UserDto>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result<UserDto>.Fail("User not found.");

        await repo.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result<UserDto>.Ok("User deleted successfully.", Map(entity));
    }
    public async Task<Result<UserProfileDto?>> GetProfile(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result<UserProfileDto?>.Fail("User not found.");

        var profile = new UserProfileDto(entity.Id, entity.Email, entity.FirstName, entity.MiddleName, entity.LastName, entity.Mobile, entity.Address, entity.ProfileImageUrl, entity.CountryId,
            entity.StateId, entity.CityId, entity.CreatedTime, entity.LastUpdatedTime);

        return Result<UserProfileDto?>.Ok(profile);
    }

    private static UserDto Map(User user)
    {
        return new(user.FirstName, user.MiddleName, user.LastName, user.Mobile, user.Address, user.ProfileImageUrl, user.CountryId, user.StateId, user.CityId);
    }
}
