using Microsoft.EntityFrameworkCore;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Entities;
using Users.Infrastructure.Db;

namespace Users.Infrastructure.Repositories;

internal sealed class MenuRepository(UserDbContext db) : IMenuRepository
{
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await db.Menus.AnyAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }
    
    public async Task<List<Menu>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await db.Menus
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }
    public async Task<Menu?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await db.Menus
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Menu?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await db.Menus
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ICollection<Menu>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        //1 CHECK IF USER IS ADMIN
        bool isAdmin = await db.UserRoles
        .Include(ur => ur.Role)
        .AnyAsync(ur => ur.UserId == userId && ur.Role.Name == "Admin", cancellationToken).ConfigureAwait(false);

        List<Menu> efMenus;

        //2 IF ADMIN --> RETURN ALL MENUS
        if (isAdmin)
        {
            efMenus = await db.Menus
                .Include(m => m.Parent)
                .OrderBy(m => m.ParentId)
                .ThenBy(m => m.Sequence)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
        }
        //3 IF NOT ADMIN --> RETURN ROLE BASED MENUS
        else
        {
            efMenus = await db.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
                .ThenInclude(r => r.RoleMenus)
                    .ThenInclude(rm => rm.Menu)
                        .ThenInclude(m => m.Parent)
            .SelectMany(ur => ur.Role.RoleMenus.Select(rm => rm.Menu))
            .Distinct()
            .OrderBy(m => m.ParentId)
            .ThenBy(m => m.Sequence)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
        }
        return efMenus;
    }

    public async Task AddAsync(Menu Menu, CancellationToken cancellationToken = default)
    {
        await db.Menus.AddAsync(Menu, cancellationToken).ConfigureAwait(false);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Menu Menu, CancellationToken cancellationToken = default)
    {
        db.Menus.Update(Menu);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Menu Menu, CancellationToken cancellationToken = default)
    {
        db.Menus.Remove(Menu);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
