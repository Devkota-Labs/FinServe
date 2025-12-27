using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Application.Dtos;
using Shared.Common.Services;
using Shared.Common.Utils;
using System.Linq.Expressions;
using Users.Application.Dtos.User;
using Users.Application.Interfaces.Services;
using Users.Domain.Entities;
using Users.Infrastructure.Db;

namespace Users.Infrastructure.Services;

internal sealed class UserReadService(ILogger logger, UserDbContext userDbContext)
    : BaseService(logger.ForContext<UserReadService>(), null), IUserReadService
{
    public async Task<AuthUserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await userDbContext.Users
        .AsNoTracking()
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
        .Where(x => x.Id == id)
            .Select(ToAuthUser())
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

    public async Task<AuthUserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await userDbContext.Users
        .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .Where(x => x.Email == email)
            .Select(ToAuthUser())
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

    public async Task<AuthUserDto?> GetByMobileAsync(string mobile, CancellationToken cancellationToken = default)
        => await userDbContext.Users
        .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .Where(x => x.Mobile == mobile)
            .Select(ToAuthUser())
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

    public async Task<AuthUserDto?> GetByUserNameOrEmailAsync(string input, CancellationToken cancellationToken = default)
        => await userDbContext.Users
        .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .Where(x => x.Email == input || x.UserName == input)
            .Select(ToAuthUser())
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        => await userDbContext.Users.AnyAsync(x => x.Email == email, cancellationToken).ConfigureAwait(false);

    public async Task<bool> MobileExistsAsync(string mobile, CancellationToken cancellationToken = default)
        => await userDbContext.Users.AnyAsync(x => x.Mobile == mobile, cancellationToken).ConfigureAwait(false);

    public async Task<bool> UserNameExistsAsync(string username, CancellationToken cancellationToken = default)
        => await userDbContext.Users.AnyAsync(x => x.UserName == username, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Returns users whose passwords will expire within the given time window.
    /// </summary>
    public async Task<IEnumerable<AuthUserDto>> GetUsersWithExpiringPasswordsAsync(TimeSpan within, CancellationToken cancellationToken = default)
    {
        var now = DateTimeUtil.Now;
        var reminderThreshold = now + within;

        return await userDbContext.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.IsActive && u.PasswordExpiryDate != null && u.PasswordExpiryDate <= reminderThreshold && u.PasswordExpiryDate > now) // Not already expired
            .Select(ToAuthUser())
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<ICollection<PendingUserDto>> GetUnApprovedUsers(CancellationToken cancellationToken = default)
    {
        return await userDbContext.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.IsActive && !u.IsApproved)
            .Select(ToPendingUser())
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<ICollection<LockedUserDto>> GetLockedUsers(CancellationToken cancellationToken = default)
    {
        return await userDbContext.Users
            .AsNoTracking()
            .Where(u => u.LockoutEndAt.HasValue && u.LockoutEndAt.Value > DateTimeUtil.Now)
            .Select(ToLockedUser())
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> IsUserNameAvailableAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await userDbContext.Users
           .AsNoTracking()
           .AnyAsync(u => u.UserName == userName, cancellationToken).ConfigureAwait(false);
    }

    // Mapping Expression
    private static Expression<Func<User, AuthUserDto>> ToAuthUser()
        => x => new AuthUserDto(x.UserRoles != null ? x.UserRoles.Select(r => r.Role.Name).ToList() : new List<string>())
        {
            Id = x.Id,
            UserName = x.UserName,
            FullName = x.FullName,
            Email = x.Email,
            MobileNo = x.Mobile,
            PasswordHash = x.PasswordHash,
            IsActive = x.IsActive,
            IsEmailVerified = x.EmailVerified,
            IsMobileVerified = x.MobileVerified,
            IsApproved = x.IsApproved,
            ProfileImageUrl = x.ProfileImageUrl,
            LockoutEndAt = x.LockoutEndAt,
            MfaEnabled = x.MfaEnabled,
            MfaSecret = x.MfaSecret,
            PasswordExpiryDate = x.PasswordExpiryDate,
        };

    private static Expression<Func<User, PendingUserDto>> ToPendingUser()
        => x => new PendingUserDto(x.Id, x.UserName, x.FullName, x.Email, x.Mobile, x.IsApproved, x.EmailVerified, x.MobileVerified, x.IsActive, x.CreatedTime);

    private static Expression<Func<User, LockedUserDto>> ToLockedUser()
        => x => new LockedUserDto(x.Id, x.FullName, x.Email, x.Mobile, x.LockoutEndAt.GetValueOrDefault());
}