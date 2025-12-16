using Asp.Versioning;
using Location.Application.Dtos;
using Location.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Api;

namespace Location.Api.Controllers;

[ApiVersion("1.0")]
public sealed class CitiesController(ILogger logger, ICityService cityService)
    : BaseApiController(logger.ForContext<CitiesController>())
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var serviceResponse = await cityService.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await cityService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("state/{stateId}")]
    public async Task<IActionResult> GetByState(int stateId, CancellationToken cancellationToken)
    {
        var serviceResponse = await cityService.GetByStateAsync(stateId, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post(CreateCityDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var serviceResponse = await cityService.CreateAsync(dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(int id, UpdateCityDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var serviceResponse = await cityService.UpdateAsync(id, dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await cityService.DeleteAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }
}