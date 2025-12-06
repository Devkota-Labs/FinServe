using Microsoft.EntityFrameworkCore;
using Users.Application.Interfaces.Repositories;
using Users.Domain.Entities;
using Users.Infrastructure.Db;

namespace Users.Infrastructure.Repositories;

internal sealed class MenuRepository(UserDbContext db) : IMenuRepository
{
    private readonly UserDbContext _db = db;

    public async Task<bool> ExistsAsync(int id)
    {
        return await _db.Menus.AnyAsync(x => x.Id == id).ConfigureAwait(false);
    }
    
    public async Task<List<Menu>> GetAllAsync()
    {
        return await _db.Menus
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync().ConfigureAwait(false);
    }
    public async Task<Menu?> GetByIdAsync(int id)
    {
        return await _db.Menus
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<Menu?> GetByNameAsync(string name)
    {
        return await _db.Menus
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .FirstOrDefaultAsync(x => x.Name == name).ConfigureAwait(false);
    }

    public async Task AddAsync(Menu Menu)
    {
        await _db.Menus.AddAsync(Menu).ConfigureAwait(false);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(Menu Menu)
    {
        _db.Menus.Update(Menu);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(Menu Menu)
    {
        _db.Menus.Remove(Menu);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }
}
