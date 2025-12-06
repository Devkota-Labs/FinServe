using Microsoft.EntityFrameworkCore;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Entities;
using Users.Infrastructure.Db;

namespace Users.Infrastructure.Repositories;

internal sealed class UserRepository(UserDbContext db) : IUserRepository
{
    private readonly UserDbContext _db = db;

    public async Task<bool> ExistsAsync(int id)
    {
        return await _db.Users.AnyAsync(x => x.Id == id).ConfigureAwait(false);
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        return await _db.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .OrderBy(x => x.Id)
            .ToListAsync().ConfigureAwait(false);
    }
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _db.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        return await _db.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.FullName == name).ConfigureAwait(false);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _db.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
    }

    public async Task<User?> GetByMobileAsync(string mobile)
    {
        return await _db.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Mobile == mobile).ConfigureAwait(false);
    }

    public async Task AddAsync(User User)
    {
        await _db.Users.AddAsync(User).ConfigureAwait(false);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(User User)
    {
        _db.Users.Update(User);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(User User)
    {
        _db.Users.Remove(User);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }
}
