using Asp.Versioning;
using Location.Application.Dtos;
using Location.Application.Interfaces.Repositories;
using Location.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Api;

namespace Location.Api.Controllers;

[ApiVersion("1.0")]
public sealed class StatesController(ILogger logger, IStateRepository stateRepository) : BaseApiController(logger.ForContext<StatesController>())
{
    private readonly IStateRepository _stateRepository = stateRepository;

    private StateDto MapToDto(State state) =>
        new(state.Id, state.Name, state.CountryId, state.Country.Name);

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _stateRepository.GetAllAsync().ConfigureAwait(false);

        var states = data.Select(MapToDto);

        return Ok(states);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var state = await _stateRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (state == null)
            return NotFound($"State not found with id {id}");

        return Ok(MapToDto(state));
    }

    [HttpGet("country/{countryId}")]
    public async Task<IActionResult> GetByCountry(int countryId)
    {
        var data = await _stateRepository.GetByCountryAsync(countryId).ConfigureAwait(false);

        if (data == null || data.Count == 0)
            return NotFound($"No states found for country id {countryId}");

        var states = data.Select(MapToDto);

        return Ok(states);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post(CreateStateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var statesByCountry = await _stateRepository.GetByCountryAsync(dto.CountryId).ConfigureAwait(false);

        var existsInCountry = statesByCountry?.FirstOrDefault(s => s.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

        if (existsInCountry is not null)
        {
            return BadRequest($"State with name {existsInCountry.Name} and country {existsInCountry.Country} already exists.");
        }

        var state = new State
        {
            Name = dto.Name,
            CountryId = dto.CountryId
        };

        await _stateRepository.AddAsync(state).ConfigureAwait(false);

        return Created("State created.", MapToDto(state));
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(int id, UpdateStateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var state = await _stateRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (state == null)
            return NotFound($"State not found with id {id}");

        if (dto.Name is not null)
        {
            var statesByCountry = await _stateRepository.GetByCountryAsync(state.CountryId).ConfigureAwait(false);

            var existsInCountry = statesByCountry?.FirstOrDefault(s => s.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

            if (existsInCountry is not null)
            {
                return BadRequest($"State with name {existsInCountry.Name} and country {existsInCountry.Country} already exists.");
            }

            state.Name = dto.Name;
        }

        if (dto.CountryId is not null) state.CountryId = dto.CountryId.Value;

        await _stateRepository.UpdateAsync(state).ConfigureAwait(false);

        return Ok("State updated.", MapToDto(state));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var state = await _stateRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (state == null)
            return NotFound($"State not found with id {id}");

        await _stateRepository.DeleteAsync(state).ConfigureAwait(false);

        return Ok("State deleted.", MapToDto(state));
    }
}
