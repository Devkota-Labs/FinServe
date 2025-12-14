using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Application.Dtos;
using Shared.Common.Services;
using System.Linq.Expressions;
using Users.Application.Interfaces.Services;
using Users.Domain.Entities;
using Users.Infrastructure.Db;

namespace Users.Infrastructure.Services;

internal sealed class UserReadService(ILogger logger, UserDbContext userDbContext) 
    : BaseService(logger.ForContext<UserReadService>(), null), IUserReadService
{
    public async Task<AuthUserDto?> GetByIdAsync(int id, CancellationToken cancellationToken= default)
        => await userDbContext.Users
        .AsNoTracking()
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
        .Where(x => x.Id == id)
            .Select(ToAuthUser())
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

    public async Task<AuthUserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken= default)
        => await userDbContext.Users
        .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .Where(x => x.Email == email)
            .Select(ToAuthUser())
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

    public async Task<AuthUserDto?> GetByMobileAsync(string mobile, CancellationToken cancellationToken= default)
        => await userDbContext.Users
        .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .Where(x => x.Mobile == mobile)
            .Select(ToAuthUser())
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

    public async Task<AuthUserDto?> GetByUserNameOrEmailAsync(string input, CancellationToken cancellationToken= default)
        => await userDbContext.Users
        .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .Where(x => x.Email == input || x.UserName == input)
            .Select(ToAuthUser())
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken= default)
        => await userDbContext.Users.AnyAsync(x => x.Email == email, cancellationToken).ConfigureAwait(false);

    public async Task<bool> MobileExistsAsync(string mobile, CancellationToken cancellationToken= default)
        => await userDbContext.Users.AnyAsync(x => x.Mobile == mobile, cancellationToken).ConfigureAwait(false);

    public async Task<bool> UserNameExistsAsync(string username, CancellationToken cancellationToken= default)
        => await userDbContext.Users.AnyAsync(x => x.UserName == username, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Returns users whose passwords will expire within the given time window.
    /// </summary>
    public async Task<IEnumerable<AuthUserDto>> GetUsersWithExpiringPasswordsAsync(TimeSpan within, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var reminderThreshold = now + within;

        return await userDbContext.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.IsActive && u.PasswordExpiryDate != null && u.PasswordExpiryDate <= reminderThreshold && u.PasswordExpiryDate > now) // Not already expired
            .Select(ToAuthUser())
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    // Mapping Expression
    private static Expression<Func<User, AuthUserDto>> ToAuthUser()
        => x => new AuthUserDto
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
            Roles = x.UserRoles.Select(r => r.Role.Name).ToList()
        };
}