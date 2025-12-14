using Location.Application.Interfaces.Repositories;
using Location.Domain.Entities;
using Location.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Location.Infrastructure.Repositories;

internal sealed class CityRepository(LocationDbContext db) : ICityRepository
{
    private readonly LocationDbContext _db = db;

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Cities.AnyAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<City>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Cities
            .AsNoTracking()
            .Include(c => c.State.Country)
            .Include(c => c.State)
            .OrderBy(x => x.State.CountryId)
            .ThenBy(x => x.StateId)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<City?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Cities
            .AsNoTracking()
            .Include(c => c.State.Country)
            .Include(c => c.State)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<City?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _db.Cities
            .AsNoTracking()
            .Include(c => c.State.Country)
            .Include(c => c.State)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<List<City>?> GetByStateAsync(int stateId, CancellationToken cancellationToken = default)
    {
        return await _db.Cities
            .AsNoTracking()
            .Include(c => c.State.Country)
            .Include(c => c.State)
            .Where(x => x.StateId == stateId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task AddAsync(City city, CancellationToken cancellationToken = default)
    {
        await _db.Cities.AddAsync(city,cancellationToken).ConfigureAwait(false);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(City city, CancellationToken cancellationToken = default)
    {
        _db.Cities.Update(city);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(City city, CancellationToken cancellationToken = default)
    {
        _db.Cities.Remove(city);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}