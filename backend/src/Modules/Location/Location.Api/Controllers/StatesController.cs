using Asp.Versioning;
using Location.Application.Dtos;
using Location.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Api;

namespace Location.Api.Controllers;

[ApiVersion("1.0")]
public sealed class StatesController(ILogger logger, IStateService stateService)
    : BaseApiController(logger.ForContext<StatesController>())
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var serviceResponse = await stateService.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await stateService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("country/{countryId}")]
    public async Task<IActionResult> GetByCountry(int countryId, CancellationToken cancellationToken)
    {
        var serviceResponse = await stateService.GetByCountryAsync(countryId, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post(CreateStateDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var serviceResponse = await stateService.CreateAsync(dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(int id, UpdateStateDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var serviceResponse = await stateService.UpdateAsync(id, dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await stateService.DeleteAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }
}
