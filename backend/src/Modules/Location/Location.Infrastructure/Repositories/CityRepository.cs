using Location.Application.Interfaces.Repositories;
using Location.Domain.Entities;
using Location.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Location.Infrastructure.Repositories;

internal sealed class CityRepository(LocationDbContext db) : ICityRepository
{
    private readonly LocationDbContext _db = db;

    public async Task<bool> ExistsAsync(int id)
    {
        return await _db.Cities.AnyAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<List<City>> GetAllAsync()
    {
        return await _db.Cities
            .AsNoTracking()
            .Include(c => c.State.Country)
            .Include(c => c.State)
            .OrderBy(x => x.State.CountryId)
            .ThenBy(x => x.StateId)
            .ThenBy(x => x.Name)
            .ToListAsync().ConfigureAwait(false);
    }

    public async Task<City?> GetByIdAsync(int id)
    {
        return await _db.Cities
            .AsNoTracking()
            .Include(c => c.State.Country)
            .Include(c => c.State)
            .FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);
    }

    public async Task<City?> GetByNameAsync(string name)
    {
        return await _db.Cities
            .AsNoTracking()
            .Include(c => c.State.Country)
            .Include(c => c.State)
            .FirstOrDefaultAsync(x => x.Name == name)
            .ConfigureAwait(false);
    }

    public async Task<List<City>?> GetByStateAsync(int stateId)
    {
        return await _db.Cities
            .AsNoTracking()
            .Include(c => c.State.Country)
            .Include(c => c.State)
            .Where(x => x.StateId == stateId)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task AddAsync(City city)
    {
        await _db.Cities.AddAsync(city).ConfigureAwait(false);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(City city)
    {
        _db.Cities.Update(city);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(City city)
    {
        _db.Cities.Remove(city);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }
}