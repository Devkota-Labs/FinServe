using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Application.Dtos;
using Shared.Common.Services;
using Shared.Common.Utils;
using Shared.Security.Configurations;
using Users.Application.Dtos.User;
using Users.Application.Interfaces.Services;
using Users.Domain.Entities;
using Users.Infrastructure.Db;

namespace Users.Infrastructure.Services;

internal sealed class UserWriteService(ILogger logger
    , UserDbContext userDbContext
    , IOptions<SecurityOptions> securityOptions
    , IOptions<LockoutOptions> lockoutOptions
    )
    : BaseService(logger.ForContext<UserWriteService>(), null)
    , IUserWriteService
{
    private readonly SecurityOptions _securityOptions = securityOptions.Value;
    private readonly LockoutOptions _lockoutOptions = lockoutOptions.Value;

    public async Task<AuthUserDto> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new User
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

            entity.AddAddress(userAddress);
        }

        userDbContext.Users.Add(entity);
        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new AuthUserDto(entity.UserRoles != null ? entity.UserRoles.Select(r => r.Role.Name).ToList() : [])
        {
            Id = entity.Id,
            UserName = entity.UserName,
            FullName = entity.FullName,
            Email = entity.Email,
            MobileNo = entity.Mobile,
            PasswordHash = entity.PasswordHash,
            IsActive = entity.IsActive,
            IsEmailVerified = entity.EmailVerified,
            IsMobileVerified = entity.MobileVerified,
            IsApproved = entity.IsApproved,
            ProfileImageUrl = entity.ProfileImageUrl,
            LockoutEndAt = entity.LockoutEndAt,
            MfaEnabled = entity.MfaEnabled,
            MfaSecret = entity.MfaSecret,
            PasswordExpiryDate = entity.PasswordExpiryDate,
        };
    }

    public async Task UpdateEmailAsync(int userId, string newEmail, CancellationToken cancellationToken = default)
    {
        var user = await userDbContext.Users.FirstAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);
        user.Email = newEmail;
        user.EmailVerified = false;

        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateMobileAsync(int userId, string newMobile, CancellationToken cancellationToken = default)
    {
        var user = await userDbContext.Users.FirstAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);
        user.Mobile = newMobile;
        user.MobileVerified = false;

        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task MarkEmailVerifiedAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userDbContext.Users.FirstAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);
        user.EmailVerified = true;

        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task MarkMobileVerifiedAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userDbContext.Users.FirstAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);
        user.MobileVerified = true;

        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task MarkFailedLogin(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userDbContext.Users.FirstAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);

        user.FailedLoginCount++;
        if (user.FailedLoginCount >= _lockoutOptions.MaxFailedAttempts)
        {
            user.LockoutEndAt = DateTimeUtil.Now.AddMinutes(_lockoutOptions.LockoutMinutes);
            user.FailedLoginCount = 0;
        }

        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task MarkSuccessLogin(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userDbContext.Users.FirstAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);

        user.FailedLoginCount = 0;

        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task SetPasswordHashAsync(int userId, string hash, CancellationToken cancellationToken = default)
    {
        var user = await userDbContext.Users.FirstAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);
        user.PasswordHash = hash;
        user.PasswordLastChanged = DateTimeUtil.Now;
        user.PasswordExpiryDate = DateTimeUtil.Now.AddDays(_securityOptions.PasswordExpiryDays);

        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task ApproveUser(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userDbContext.Users.FirstAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);
        user.IsApproved = true;

        await AssignRoles(userId, [1], cancellationToken).ConfigureAwait(false);

        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UnlockUser(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userDbContext.Users.FirstAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);

        // Update fields
        user.LockoutEndAt = null;
        user.FailedLoginCount = 0;

        //ToDo LoginHistory should be updated
        //// Create a login history entry if the table exists in your DbContext
        //var history = new LoginHistory
        //{
        //    UserId = user.Id,
        //    LogoutTime = null,
        //    IpAddress = ip,
        //    //Device = Request.Headers["User-Agent"].ToString(),
        //    Status = Status.SUCCESS,
        //    //Message = "Account unlocked by admin"
        //};

        //loginHistoryService.LogoutAsync()

        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task AssignRoles(int userId, ICollection<int> roleIds, CancellationToken cancellationToken = default)
    {
        // remove existing roles
        var existing = await userDbContext.UserRoles.Where(ur => ur.UserId == userId).ToListAsync(cancellationToken).ConfigureAwait(false);
        userDbContext.UserRoles.RemoveRange(existing);

        // add new roles
        foreach (var roleId in roleIds)
        {
            userDbContext.UserRoles.Add(new UserRole
            {
                UserId = userId,
                RoleId = roleId
            });
        }

        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}