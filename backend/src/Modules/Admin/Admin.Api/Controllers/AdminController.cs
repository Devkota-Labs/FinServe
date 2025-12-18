using Admin.Application.Dtos;
using Admin.Application.Interfaces.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Api;

namespace Admin.Api.Controllers;

[ApiVersion("1.0")]
[Authorize(Roles = "Admin")]
public sealed class AdminController(ILogger logger, IAdminService adminService)
    : BaseApiController(logger.ForContext<AdminController>())
{
    [HttpGet("pending-users")]
    public async Task<IActionResult> GetPendingUsers(CancellationToken cancellationToken)
    {
        var serviceResponse = await adminService.GetUnApprovedUsersAsync(cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("approve/{userId}")]
    public async Task<IActionResult> ApproveUser(int userId, CancellationToken cancellationToken)
    {
        var serviceResponse = await adminService.ApproveUser(userId, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("assign/{userId}")]
    public async Task<IActionResult> AssignRoles(int userId, [FromBody] AssignRoleDto dto, CancellationToken cancellationToken)
    {
        var serviceResponse = await adminService.AssignRoles(userId, dto, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }
    /// <summary>
    /// Unlock a user account (clear lockout and reset failed attempts).
    /// Only Admin can call.
    /// </summary>
    [HttpPatch("unlock/{userId}")]
    public async Task<IActionResult> UnlockUser(int userId, CancellationToken cancellationToken)
    {
        var serviceResponse = await adminService.UnlockUser(userId, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    //[HttpDelete("cleanup-reset-tokens")]
    //public async Task<IActionResult> Cleanup([FromServices] AppDbContext db)
    //{
    //    var expired = db.PasswordResetTokens.Where(t => t.ExpiresAt < DateTime.UtcNow);
    //    db.PasswordResetTokens.RemoveRange(expired);
    //    await db.SaveChangesAsync().ConfigureAwait(false);

    //    return Ok("Expired tokens removed.");
    //}
}