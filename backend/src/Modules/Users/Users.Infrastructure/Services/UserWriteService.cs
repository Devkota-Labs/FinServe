using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Shared.Application.Dtos;
using Shared.Common.Services;
using Users.Application.Dtos.User;
using Users.Application.Interfaces.Services;
using Users.Domain.Entities;
using Users.Infrastructure.Db;

namespace Users.Infrastructure.Services;

internal sealed class UserWriteService(ILogger logger, UserDbContext userDbContext, IConfiguration configuration)
    : BaseService(logger.ForContext<UserWriteService>(), null), IUserWriteService
{
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

        userDbContext.Users.Add(entity);
        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new AuthUserDto
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
            Roles = entity.UserRoles != null ?  entity.UserRoles.Select(r => r.Role.Name).ToList() : []
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
        if (user.FailedLoginCount >= configuration.GetValue("Security:Lockout:MaxFailedAttempts", 5))
        {
            user.LockoutEndAt = DateTime.UtcNow.AddMinutes(configuration.GetValue("Security:Lockout:LockoutMinutes", 15));
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
        user.PasswordLastChanged = DateTime.UtcNow;
        user.PasswordExpiryDate = DateTime.UtcNow.AddDays(configuration.GetValue("AppConfig:Security:PasswordExpiryDays", 90));

        await userDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}