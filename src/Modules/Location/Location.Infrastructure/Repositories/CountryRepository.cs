using Location.Application.Interfaces.Repositories;
using Location.Domain.Entities;
using Location.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Location.Infrastructure.Repositories;

internal sealed class CountryRepository(LocationDbContext db) : ICountryRepository
{
    private readonly LocationDbContext _db = db;

    public async Task<bool> ExistsAsync(int id)
    {
        return await _db.Countries.AnyAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<List<Country>> GetAllAsync()
    {
        return await _db.Countries
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync().ConfigureAwait(false);
    }
    public async Task<Country?> GetByIdAsync(int id)
    {
        return await _db.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<Country?> GetByNameAsync(string name)
    {
        return await _db.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name).ConfigureAwait(false);
    }

    public async Task AddAsync(Country country)
    {
        await _db.Countries.AddAsync(country).ConfigureAwait(false);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(Country country)
    {
        _db.Countries.Update(country);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(Country country)
    {
        _db.Countries.Remove(country);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }
}
