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
public sealed class CitiesController(ILogger logger, ICityRepository cityRepository) : BaseApiController(logger.ForContext<CitiesController>())
{
    private readonly ICityRepository _cityRepository = cityRepository;

    private CityDto MapToDto(City city) =>
        new(city.Id, city.Name, city.StateId, city.State.Name, city.State.CountryId, city.State.Country.Name);

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _cityRepository.GetAllAsync().ConfigureAwait(false);

        var cities = data.Select(MapToDto);

        return Ok(cities);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (city == null)
            return NotFound($"City not found with id {id}");

        return Ok(MapToDto(city));
    }

    [HttpGet("state/{stateId}")]
    public async Task<IActionResult> GetByState(int stateId)
    {
        var data = await _cityRepository.GetByStateAsync(stateId).ConfigureAwait(false);

        if (data == null || data.Count == 0)
            return NotFound($"No cities found for state id {stateId}");

        var cities = data.Select(MapToDto);

        return Ok(cities);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post(CreateCityDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var citiesByState = await _cityRepository.GetByStateAsync(dto.StateId).ConfigureAwait(false);

        var existsInState = citiesByState?.FirstOrDefault(s => s.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

        if (existsInState is not null)
        {
            return BadRequest($"City with name {existsInState.Name} and state {existsInState.State} already exists.");
        }

        var city = new City
        {
            Name = dto.Name,
            StateId = dto.StateId
        };

        await _cityRepository.AddAsync(city).ConfigureAwait(false);

        return Created("City created.", MapToDto(city));
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(int id, UpdateCityDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var city = await _cityRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (city == null)
            return NotFound($"City not found with id {id}");

        if (dto.Name is not null)
        {
            var citiesByState = await _cityRepository.GetByStateAsync(city.StateId).ConfigureAwait(false);

            var existsInState = citiesByState?.FirstOrDefault(s => s.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

            if (existsInState is not null)
            {
                return BadRequest($"City with name {existsInState.Name} and state {existsInState.State} already exists.");
            }

            city.Name = dto.Name;
        }

        if (dto.StateId is not null) city.StateId = dto.StateId.Value;

        await _cityRepository.UpdateAsync(city).ConfigureAwait(false);

        return Ok("City updated.", MapToDto(city));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (city == null)
            return NotFound($"City not found with id {id}");

        await _cityRepository.DeleteAsync(city).ConfigureAwait(false);

        return Ok("City deleted.", MapToDto(city));
    }
}
