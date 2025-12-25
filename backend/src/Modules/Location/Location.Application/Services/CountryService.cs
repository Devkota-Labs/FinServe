using Location.Application.Dtos;
using Location.Application.Interfaces.Repositories;
using Location.Application.Interfaces.Services;
using Location.Domain.Entities;
using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;

namespace Location.Application.Services;

internal sealed class CountryService(ILogger logger, ICountryRepository repo)
    : BaseService(logger.ForContext<CountryService>(), null), ICountryService
{
    public async Task<Result<ICollection<CountryDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repo.GetAllAsync(cancellationToken).ConfigureAwait(false);

        var result = entities.Select(Map).ToList();

        return Result.Ok<ICollection<CountryDto>>(result);
    }

    public async Task<Result<CountryDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entities = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entities == null)
            return Result.Fail<CountryDto>("Country not found");

        return Result.Ok(Map(entities));
    }

    public async Task<Result<CountryDto>> CreateAsync(CreateCountryDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByNameAsync(dto.Name, cancellationToken).ConfigureAwait(false);

        if (entity is not null)
        {
            return Result.Fail<CountryDto>($"Country with name {entity.Name} already exists.");
        }

        var newEntity = new Country
        {
            Name = dto.Name,
            IsoCode = dto.IsoCode,
            MobileCode = dto.MobileCode
        };

        await repo.AddAsync(newEntity, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Country created successfully.", Map(newEntity));
    }

    public async Task<Result<CountryDto>> UpdateAsync(int id, UpdateCountryDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<CountryDto>("Country not found.");

        if (dto.Name is not null)
        {
            var exists = await repo.GetByNameAsync(dto.Name, cancellationToken).ConfigureAwait(false);

            if (exists is not null)
            {
                return Result.Fail<CountryDto>($"Country with name {exists.Name} already exists.");
            }

            entity.Name = dto.Name;
        }

        if (dto.IsoCode is not null) entity.IsoCode = dto.IsoCode;
        if (dto.MobileCode is not null) entity.MobileCode = dto.MobileCode;

        await repo.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Country updated successfully", Map(entity));
    }

    public async Task<Result<CountryDto>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result.Fail<CountryDto>("Country not found.");

        await repo.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result.Ok("Country deleted successfully.", Map(entity));
    }

    private static CountryDto Map(Country country)
    {
        return new(country.Id, country.Name, country.IsoCode, country.MobileCode);
    }
}