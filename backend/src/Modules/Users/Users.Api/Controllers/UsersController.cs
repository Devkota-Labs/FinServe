using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Api;
using Shared.Common;
using Users.Application.Dtos.User;
using Users.Application.Interfaces.Services;

namespace Users.Api.Controllers;

[ApiVersion("1.0")]
[Authorize]
public sealed class UsersController(ILogger logger, IUserService userService) 
    : BaseApiController(logger.ForContext<UsersController>())
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var serviceResponse = await userService.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await userService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var serviceResponse = await userService.GetProfile(userId, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateUserDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest("Email/password required");

        var serviceResponse = await userService.CreateAsync(dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("profile")]
    public async Task<IActionResult> Put(UpdateUserDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var userId = GetCurrentUserId();

        var serviceResponse = await userService.UpdateAsync(userId, dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await userService.DeleteAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("addresses/{addressId}")]
    public async Task<IActionResult> UpdateAddress(int addressId, UpdateAddressDto dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var userId = GetCurrentUserId();

        var serviceResponse = await userService.UpdateAddressAsync(userId, addressId, dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpDelete("addresses/{addressId}")]
    public async Task<IActionResult> DeleteAddress(int addressId, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var serviceResponse = await userService.DeleteAddressAsync(userId, addressId, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    // Helper method
    private int GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        return claim != null ? int.Parse(claim.Value, Constants.IFormatProvider) : 0;
    }
}