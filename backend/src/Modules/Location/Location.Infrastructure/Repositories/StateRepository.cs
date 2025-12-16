using Location.Application.Interfaces.Repositories;
using Location.Domain.Entities;
using Location.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Location.Infrastructure.Repositories;

internal sealed class StateRepository(LocationDbContext db) : IStateRepository
{
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await db.States.AnyAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<State>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await db.States
            .AsNoTracking()
            .Include(c => c.Country)
            .OrderBy(x => x.CountryId)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<State?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await db.States
            .AsNoTracking()
            .Include(c => c.Country)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<State?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await db.States
            .AsNoTracking()
            .Include(c => c.Country)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<List<State>?> GetByCountryAsync(int countryId, CancellationToken cancellationToken = default)
    {
        return await db.States
            .AsNoTracking()
            .Include(c => c.Country)
            .Where(x => x.CountryId == countryId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task AddAsync(State state, CancellationToken cancellationToken = default)
    {
        await db.States.AddAsync(state, cancellationToken).ConfigureAwait(false);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(State state, CancellationToken cancellationToken = default)
    {
        db.States.Update(state);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(State state, CancellationToken cancellationToken = default)
    {
        db.States.Remove(state);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}