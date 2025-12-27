using Location.Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;
using Shared.Common.Utils;
using Shared.Security;
using Shared.Security.Configurations;
using Users.Application.Dtos.User;
using Users.Application.Interfaces.Repositories;
using Users.Application.Interfaces.Services;
using Users.Domain.Entities;

namespace Users.Application.Services;

internal sealed class UserService(ILogger logger
    , IUserRepository repo
    , IPasswordPolicyService passwordPolicyService
    , ILocationLookupService locationLookupService
    , IOptions<SecurityOptions> securityOptions)
    : BaseService(logger.ForContext<UserService>(), null), IUserService
{
    private readonly SecurityOptions _securityOptions = securityOptions.Value;
    public async Task<Result<ICollection<UserDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repo.GetAllAsync(cancellationToken).ConfigureAwait(false);

        var userDtos = new List<UserDto>();

        foreach (var user in entities)
        {
            var userDto = await Map(user, cancellationToken).ConfigureAwait(false);
            userDtos.Add(userDto);
        }

        return Result.Ok<ICollection<UserDto>>(userDtos);
    }

    public async Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entities = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entities == null)
            return Result.Fail<UserDto>($"User not found with id {id}");

        return Result.Ok(await Map(entities, cancellationToken).ConfigureAwait(false));
    }

    public async Task<Result<UserDto>> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return Result.Fail<UserDto>("Email or password required.");

        var (valid, message) = passwordPolicyService.ValidatePassword(dto.Password);

        if (!valid)
            return Result.Fail<UserDto>(message);

        var emailExists = await repo.GetByEmailAsync(dto.Email, cancellationToken).ConfigureAwait(false);

        if (emailExists != null)
            return Result.Fail<UserDto>("Email already exists.");

        var userNameExists = await repo.GetByNameAsync(dto.UserName, cancellationToken).ConfigureAwait(false);

        if (userNameExists is not null)
        {
            return Result.Fail<UserDto>($"User with name {userNameExists.UserName} already exists.");
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
            IsActive = true,
            IsApproved = false,
            PasswordHash = dto.Password,
            PasswordLastChanged = DateTimeUtil.Now,
            PasswordExpiryDate = DateTimeUtil.Now.AddDays(_securityOptions.PasswordExpiryDays)
        };

        foreach (var item in dto.Address)
        {
            var userAddress = new UserAddress
            {
                AddressType = item.AddressType,
                AddressLine1 = item.AddressLine1,
                AddressLine2 = item.AddressLine2,
                CityId = item.CityId,
                StateId = item.StateId,
                CountryId = item.CountryId,
                PinCode = item.PinCode,
                IsPrimary = item.IsPrimary
            };

            newEntity.AddAddress(userAddress);
        }

        await repo.AddAsync(newEntity, cancellationToken).ConfigureAwait(false);

        return Result.Ok("User created successfully.", await Map(newEntity, cancellationToken).ConfigureAwait(false));
    }

    public async Task<Result<UserDto>> UpdateAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<UserDto>("User not found.");

        // Basic info
        if (dto.FirstName is not null) entity.FirstName = dto.FirstName;
        if (dto.MiddleName is not null) entity.MiddleName = dto.MiddleName;
        if (dto.LastName is not null) entity.LastName = dto.LastName;
        if (dto.ProfileImageUrl is not null) entity.ProfileImageUrl = dto.ProfileImageUrl;

        entity.LastUpdatedTime = DateTimeUtil.Now;

        await repo.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result.Ok("User updated successfully", await Map(entity, cancellationToken).ConfigureAwait(false));
    }

    public async Task<Result<UserDto>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id is 1)
            return Result.Fail<UserDto>("Default user can not be deleted.");

        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<UserDto>("User not found.");

        await repo.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result.Ok("User deleted successfully.", await Map(entity, cancellationToken).ConfigureAwait(false));
    }

    public async Task<Result<UserProfileDto>> GetProfile(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<UserProfileDto>("User not found.");

        var entities = await repo.GetAddressAsync(id, cancellationToken).ConfigureAwait(false);

        var addressDtos = new List<AddressDto>();
        foreach (var address in entities)
        {
            var addressDto = await MapAddress(address, cancellationToken).ConfigureAwait(false);
            addressDtos.Add(addressDto);
        }

        var profile = new UserProfileDto(entity.Id, entity.Email, entity.FirstName, entity.MiddleName, entity.LastName, entity.Mobile, entity.ProfileImageUrl, addressDtos, entity.CreatedTime, entity.LastUpdatedTime);

        return Result.Ok(profile);
    }

    public async Task<Result<ICollection<AddressDto>>> GetAddressAsync(int userId, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<ICollection<AddressDto>>("User not found.");

        var entities = await repo.GetAddressAsync(userId, cancellationToken).ConfigureAwait(false);

        var addressDtos = new List<AddressDto>();
        foreach (var address in entities)
        {
            var addressDto = await MapAddress(address, cancellationToken).ConfigureAwait(false);
            addressDtos.Add(addressDto);
        }

        return Result.Ok<ICollection<AddressDto>>(addressDtos);
    }

    public async Task<Result<AddressDto>> UpdateAddressAsync(int id, int addressId, UpdateAddressDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<AddressDto>("User not found.");

        var address = await repo.GetAddressByIdAsync(id, addressId, cancellationToken).ConfigureAwait(false);

        if (address == null)
            return Result.Fail<AddressDto>("Address not found.");

        if (dto.AddressType is not null) address.AddressType = dto.AddressType.Value;
        if (dto.AddressLine1 is not null) address.AddressLine1 = dto.AddressLine1;
        if (dto.AddressLine2 is not null) address.AddressLine2 = dto.AddressLine2;
        if (dto.CountryId is not null) address.CountryId = dto.CountryId.Value;
        if (dto.StateId is not null) address.StateId = dto.StateId.Value;
        if (dto.CityId is not null) address.CityId = dto.CityId.Value;
        if (dto.PinCode is not null) address.PinCode = dto.PinCode;
        if (dto.IsPrimary is not null && dto.IsPrimary.Value)
        {
            // Set other addresses as non-primary
            foreach (var addr in entity.Addresses)
            {
                addr.UnsetPrimary();
            }
            address.SetPrimary();
        }

        await repo.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var addressDto = await MapAddress(address, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Address updated successfully.", addressDto);
    }

    public async Task<Result<AddressDto>> DeleteAddressAsync(int id, int addressId, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<AddressDto>("User not found.");

        var address = await repo.GetAddressByIdAsync(id, addressId, cancellationToken).ConfigureAwait(false);

        if (address == null)
            return Result.Fail<AddressDto>("Address not found.");

        await repo.DeleteAddressAsync(address, cancellationToken).ConfigureAwait(false);

        var addressDto = await MapAddress(address, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Address deleted successfully.", addressDto);
    }

    private async Task<AddressDto> MapAddress(UserAddress address, CancellationToken cancellationToken)
    {
        var location = await locationLookupService.GetLocationNamesAsync(
            address.CountryId,
            address.StateId,
            address.CityId, cancellationToken).ConfigureAwait(false);

        return new(address.Id, address.AddressType, address.AddressLine1, address.AddressLine2, address.CountryId, location.Country, address.StateId, location.State, address.CityId, location.City, address.PinCode, address.IsPrimary);
    }

    private async Task<UserDto> Map(User user, CancellationToken cancellationToken)
    {
        var addressDtos = new List<AddressDto>();
        foreach (var address in user.Addresses)
        {
            var addressDto = await MapAddress(address, cancellationToken).ConfigureAwait(false);
            addressDtos.Add(addressDto);
        }

        return new(user.Id, user.FirstName, user.MiddleName, user.LastName, user.Mobile, user.ProfileImageUrl, addressDtos);
    }
}
