using Microsoft.EntityFrameworkCore;
using System.Data;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Entities;
using Users.Infrastructure.Db;

namespace Users.Infrastructure.Repositories;

internal sealed class RoleRepository(UserDbContext db) : IRoleRepository
{
    private readonly UserDbContext _db = db;

    public async Task<bool> ExistsAsync(int id)
    {
        return await _db.Roles.AnyAsync(x => x.Id == id).ConfigureAwait(false);
    }
    
    public async Task<List<Role>> GetAllAsync()
    {
        return await _db.Roles
            .AsNoTracking()
            .Include(r => r.RoleMenus)!
             .ThenInclude(rm => rm.Menu)
            .OrderBy(x => x.Name)
            .ToListAsync().ConfigureAwait(false);
    }
    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _db.Roles
            .AsNoTracking()
            .Include(r => r.RoleMenus)!
             .ThenInclude(rm => rm.Menu)
            .OrderBy(x => x.Name)
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _db.Roles
            .AsNoTracking()
            .Include(r => r.RoleMenus)!
             .ThenInclude(rm => rm.Menu)
            .OrderBy(x => x.Name)
            .FirstOrDefaultAsync(x => x.Name == name).ConfigureAwait(false);
    }

    public async Task<ICollection<Menu>> GetMenusAsync(int roleId)
    {
        var role = await _db.Roles
         .AsNoTracking()
         .Include(r => r.RoleMenus)!
             .ThenInclude(rm => rm.Menu)
         .FirstOrDefaultAsync(r => r.Id == roleId).ConfigureAwait(false);

        return role?.RoleMenus?.Select(rm => rm.Menu!).ToList() ?? [];
    }

    public async Task AssignMenusAsync(int roleId, ICollection<int> menuIds)
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

        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task AddAsync(Role Role)
    {
        await _db.Roles.AddAsync(Role).ConfigureAwait(false);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(Role Role)
    {
        _db.Roles.Update(Role);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(Role Role)
    {
        _db.Roles.Remove(Role);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }
}
