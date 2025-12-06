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
public sealed class CountriesController(ILogger logger, ICountryRepository countryRepository) : BaseApiController(logger.ForContext<CountriesController>())
{
    private readonly ICountryRepository _countryRepository = countryRepository;

    private CountryDto MapToDto(Country country) =>
        new(country.Id, country.Name, country.IsoCode, country.MobileCode);

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _countryRepository.GetAllAsync().ConfigureAwait(false);

        var countries = data.Select(MapToDto);

        return Ok(countries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var country = await _countryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (country == null)
            return NotFound($"Country not found with id {id}");

        return Ok(MapToDto(country));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post(CreateCountryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var exists = await _countryRepository.GetByNameAsync(dto.Name).ConfigureAwait(false);

        if (exists is not null)
        {
            return BadRequest($"Country with name {exists.Name} already exists.");
        }

        var country = new Country
        {
            Name = dto.Name,
            IsoCode = dto.IsoCode,
            MobileCode = dto.MobileCode
        };
        await _countryRepository.AddAsync(country).ConfigureAwait(false);

        return Created("Country created.", MapToDto(country));
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(int id, UpdateCountryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var country = await _countryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (country == null)
            return NotFound($"Country not found with id {id}");

        if (dto.Name is not null)
        {
            var exists = await _countryRepository.GetByNameAsync(dto.Name).ConfigureAwait(false);

            if (exists is not null)
            {
                return BadRequest($"Country with name {exists.Name} already exists.");
            }

            country.Name = dto.Name;
        }
        if (dto.IsoCode is not null) country.IsoCode = dto.IsoCode;
        if (dto.MobileCode is not null) country.MobileCode = dto.MobileCode;

        await _countryRepository.UpdateAsync(country).ConfigureAwait(false);

        return Ok("Country updated.", MapToDto(country));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var country = await _countryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (country == null)
            return NotFound($"Country not found with id {id}");

        await _countryRepository.DeleteAsync(country).ConfigureAwait(false);

        return Ok("Country deleted.", MapToDto(country));
    }
}
