using Location.Application.Interfaces.Repositories;
using Location.Domain.Entities;
using Location.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Location.Infrastructure.Repositories;

internal sealed class CountryRepository(LocationDbContext db) : ICountryRepository
{
    private readonly LocationDbContext _db = db;

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Countries.AnyAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<Country>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Countries
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }
    public async Task<Country?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Country?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _db.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken).ConfigureAwait(false);
    }

    public async Task AddAsync(Country country, CancellationToken cancellationToken = default)
    {
        await _db.Countries.AddAsync(country, cancellationToken).ConfigureAwait(false);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Country country, CancellationToken cancellationToken = default)
    {
        _db.Countries.Update(country);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Country country, CancellationToken cancellationToken = default)
    {
        _db.Countries.Remove(country);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}