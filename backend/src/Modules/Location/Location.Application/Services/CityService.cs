using Location.Application.Dtos;
using Location.Application.Interfaces.Repositories;
using Location.Application.Interfaces.Services;
using Location.Domain.Entities;
using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;

namespace Location.Application.Services;

internal sealed class CityService(ILogger logger, ICityRepository repo)
    : BaseService(logger.ForContext<CityService>(), null), ICityService
{
    public async Task<Result<ICollection<CityDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repo.GetAllAsync(cancellationToken).ConfigureAwait(false);

        var result = entities.Select(Map).ToList();

        return Result<ICollection<CityDto>>.Ok(result);
    }

    public async Task<Result<CityDto?>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entities = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entities == null)
            return Result<CityDto?>.Fail("City not found");

        return Result<CityDto?>.Ok(Map(entities));
    }

    public async Task<Result<ICollection<CityDto>?>> GetByStateAsync(int stateId, CancellationToken cancellationToken)
    {
        var entities = await repo.GetByStateAsync(stateId, cancellationToken).ConfigureAwait(false);

        if (entities == null || entities.Count == 0)
            return Result<ICollection<CityDto>?>.Fail($"No cities found for state id {stateId}");

        var result = entities.Select(Map).ToList();

        return Result<ICollection<CityDto>?>.Ok(result);
    }

    public async Task<Result<CityDto>> CreateAsync(CreateCityDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByStateAsync(dto.StateId, cancellationToken).ConfigureAwait(false);

        var exists = entity?.FirstOrDefault(s => s.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

        if (exists is not null)
        {
            return Result<CityDto>.Fail($"City with name {exists.Name} and state {exists.State} already exists.");
        }

        var newEntity = new City
        {
            Name = dto.Name,
            StateId = dto.StateId
        };

        await repo.AddAsync(newEntity, cancellationToken).ConfigureAwait(false);

        return Result<CityDto>.Ok("City created successfully.", Map(newEntity));
    }

    public async Task<Result<CityDto>> UpdateAsync(int id, UpdateCityDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result<CityDto>.Fail("City not found.");

        if (dto.Name is not null)
        {
            var entities = await repo.GetByStateAsync(entity.StateId, cancellationToken).ConfigureAwait(false);

            var exists = entities?.FirstOrDefault(s => s.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

            if (exists is not null)
            {
                return Result<CityDto>.Fail($"City with name {exists.Name} and state {exists.State.Name} already exists.");
            }

            entity.Name = dto.Name;
        }

        if (dto.StateId is not null) entity.StateId = dto.StateId.Value;

        await repo.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result<CityDto>.Ok("City updated successfully", Map(entity));
    }

    public async Task<Result<CityDto>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result<CityDto>.Fail("City not found.");

        await repo.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result<CityDto>.Ok("City deleted successfully.", Map(entity));
    }

    private static CityDto Map(City city)
    {
        return new(city.Id, city.Name, city.StateId, city.State?.Name, city.State != null ? city.State.CountryId : 0, city.State?.Country != null ? city.State.Country.Name : "");
    }
}