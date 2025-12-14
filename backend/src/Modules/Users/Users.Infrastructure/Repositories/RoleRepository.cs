using Microsoft.EntityFrameworkCore;
using System.Data;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Entities;
using Users.Infrastructure.Db;

namespace Users.Infrastructure.Repositories;

internal sealed class RoleRepository(UserDbContext db) : IRoleRepository
{
    private readonly UserDbContext _db = db;

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Roles.AnyAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }
    
    public async Task<List<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Roles
            .AsNoTracking()
            .Include(r => r.RoleMenus)!
             .ThenInclude(rm => rm.Menu)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }
    public async Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Roles
            .AsNoTracking()
            .Include(r => r.RoleMenus)!
             .ThenInclude(rm => rm.Menu)
            .OrderBy(x => x.Name)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _db.Roles
            .AsNoTracking()
            .Include(r => r.RoleMenus)!
             .ThenInclude(rm => rm.Menu)
            .OrderBy(x => x.Name)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ICollection<Menu>> GetMenusAsync(int roleId, CancellationToken cancellationToken = default)
    {
        var role = await _db.Roles
         .AsNoTracking()
         .Include(r => r.RoleMenus)!
             .ThenInclude(rm => rm.Menu)
         .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken).ConfigureAwait(false);

        return role?.RoleMenus?.Select(rm => rm.Menu!).ToList() ?? [];
    }

    public async Task AssignMenusAsync(int roleId, ICollection<int> menuIds, CancellationToken cancellationToken = default)
    {
        // Remove old assignments
        var old = _db.RoleMenus.Where(rm => rm.RoleId == roleId);
        _db.RoleMenus.RemoveRange(old);

        // Add new ones
        foreach (var menuId in menuIds)
        {
            _db.RoleMenus.Add(new RoleMenu
            {
                RoleId = roleId,
                MenuId = menuId
            });
        }

        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task AddAsync(Role Role, CancellationToken cancellationToken = default)
    {
        await _db.Roles.AddAsync(Role, cancellationToken).ConfigureAwait(false);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Role Role, CancellationToken cancellationToken = default)
    {
        _db.Roles.Update(Role);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Role Role, CancellationToken cancellationToken = default)
    {
        _db.Roles.Remove(Role);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
