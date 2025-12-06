using Location.Application.Interfaces.Repositories;
using Location.Domain.Entities;
using Location.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Location.Infrastructure.Repositories;

internal sealed class StateRepository(LocationDbContext db) : IStateRepository
{
    private readonly LocationDbContext _db = db;

    public async Task<bool> ExistsAsync(int id)
    {
        return await _db.States.AnyAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<List<State>> GetAllAsync()
    {
        return await _db.States
            .AsNoTracking()
            .Include(c => c.Country)
            .OrderBy(x => x.CountryId)
            .ThenBy(x => x.Name)
            .ToListAsync().ConfigureAwait(false);
    }

    public async Task<State?> GetByIdAsync(int id)
    {
        return await _db.States
            .AsNoTracking()
            .Include(c => c.Country)
            .FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);
    }

    public async Task<State?> GetByNameAsync(string name)
    {
        return await _db.States
            .AsNoTracking()
            .Include(c => c.Country)
            .FirstOrDefaultAsync(x => x.Name == name)
            .ConfigureAwait(false);
    }

    public async Task<List<State>?> GetByCountryAsync(int countryId)
    {
        return await _db.States
            .AsNoTracking()
            .Include(c => c.Country)
            .Where(x => x.CountryId == countryId)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task AddAsync(State state)
    {
        await _db.States.AddAsync(state).ConfigureAwait(false);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(State state)
    {
        _db.States.Update(state);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(State state)
    {
        _db.States.Remove(state);
        await _db.SaveChangesAsync().ConfigureAwait(false);
    }
}