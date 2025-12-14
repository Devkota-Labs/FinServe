using Asp.Versioning;
using Location.Application.Dtos;
using Location.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Api;

namespace Location.Api.Controllers;

[ApiVersion("1.0")]
public sealed class CountriesController(ILogger logger, ICountryService countryService)
    : BaseApiController(logger.ForContext<CountriesController>())
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var serviceResponse = await countryService.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await countryService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post(CreateCountryDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var serviceResponse = await countryService.CreateAsync(dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(int id, UpdateCountryDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var serviceResponse = await countryService.UpdateAsync(id, dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await countryService.DeleteAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }
}
