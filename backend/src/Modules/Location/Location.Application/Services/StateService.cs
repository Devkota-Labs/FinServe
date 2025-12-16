using Location.Application.Dtos;
using Location.Application.Interfaces.Repositories;
using Location.Application.Interfaces.Services;
using Location.Domain.Entities;
using Serilog;
using Shared.Application.Results;
using Shared.Common.Services;

namespace Location.Application.Services;

internal sealed class StateService(ILogger logger, IStateRepository repo)
    : BaseService(logger.ForContext<StateService>(), null), IStateService
{
    public async Task<Result<ICollection<StateDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await repo.GetAllAsync(cancellationToken).ConfigureAwait(false);

        var result = entities.Select(Map).ToList();

        return Result<ICollection<StateDto>>.Ok(result);
    }

    public async Task<Result<StateDto?>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entities = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entities == null)
            return Result<StateDto?>.Fail("State not found");

        return Result<StateDto?>.Ok(Map(entities));
    }

    public async Task<Result<ICollection<StateDto>?>> GetByCountryAsync(int countryId, CancellationToken cancellationToken)
    {
        var entities = await repo.GetByCountryAsync(countryId, cancellationToken).ConfigureAwait(false);

        if (entities == null || entities.Count == 0)
            return Result<ICollection<StateDto>?>.Fail($"No states found for country id {countryId}");

        var result = entities.Select(Map).ToList();

        return Result<ICollection<StateDto>?>.Ok(result);
    }

    public async Task<Result<StateDto>> CreateAsync(CreateStateDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByCountryAsync(dto.CountryId, cancellationToken).ConfigureAwait(false);

        var exists = entity?.FirstOrDefault(s => s.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

        if (exists is not null)
        {
            return Result<StateDto>.Fail($"State with name {exists.Name} and country {exists.Country.Name} already exists.");
        }

        var newEntity = new State
        {
            Name = dto.Name,
            CountryId = dto.CountryId
        };

        await repo.AddAsync(newEntity, cancellationToken).ConfigureAwait(false);

        return Result<StateDto>.Ok("State created successfully.", Map(newEntity));
    }

    public async Task<Result<StateDto>> UpdateAsync(int id, UpdateStateDto dto, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result<StateDto>.Fail("State not found.");

        if (dto.Name is not null)
        {
            var entities = await repo.GetByCountryAsync(entity.CountryId, cancellationToken).ConfigureAwait(false);

            var exists = entities?.FirstOrDefault(s => s.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

            if (exists is not null)
            {
                return Result<StateDto>.Fail($"State with name {exists.Name} and country {exists.Country.Name} already exists.");
            }

            entity.Name = dto.Name;
        }

        if (dto.CountryId is not null) entity.CountryId = dto.CountryId.Value;

        await repo.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result<StateDto>.Ok("State updated successfully", Map(entity));
    }

    public async Task<Result<StateDto>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity == null)
            return Result<StateDto>.Fail("State not found.");

        await repo.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);

        return Result<StateDto>.Ok("State deleted successfully.", Map(entity));
    }

    private static StateDto Map(State state)
    {
        return new(state.Id, state.Name, state.CountryId, state.Country != null ? state.Country.Name : "");
    }
}