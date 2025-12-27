using Microsoft.EntityFrameworkCore;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Entities;
using Users.Infrastructure.Db;

namespace Users.Infrastructure.Repositories;

internal sealed class UserRepository(UserDbContext db) : IUserRepository
{
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await db.Users.AnyAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await db.Users
            .AsNoTracking()
            .Include(x => x.Addresses)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }
    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await db.Users
            .AsNoTracking()
            .Include(x => x.Addresses)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<User?> GetByNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await db.Users
            .AsNoTracking()
            .Include(x => x.Addresses)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken).ConfigureAwait(false);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await db.Users
            .AsNoTracking()
            .Include(x => x.Addresses)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken).ConfigureAwait(false);
    }

    public async Task<User?> GetByMobileAsync(string mobile, CancellationToken cancellationToken = default)
    {
        return await db.Users
            .AsNoTracking()
            .Include(x => x.Addresses)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Mobile == mobile, cancellationToken).ConfigureAwait(false);
    }

    public async Task<User?> GetByUserNameOrEmailAsync(string userNameOrEmail, CancellationToken cancellationToken = default)
        => await db.Users.Where(x => x.Email == userNameOrEmail || x.UserName == userNameOrEmail)
            .AsNoTracking()
            .Include(x => x.Addresses)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await db.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        db.Users.Remove(user);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<UserAddress?> GetAddressByIdAsync(int userId, int addressId, CancellationToken cancellationToken = default)
    {
        return await db.UserAddresses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == addressId && x.UserId == userId, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ICollection<UserAddress>> GetAddressAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await db.UserAddresses
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAddressAsync(UserAddress userAddress, CancellationToken cancellationToken = default)
    {
        db.UserAddresses.Remove(userAddress);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
