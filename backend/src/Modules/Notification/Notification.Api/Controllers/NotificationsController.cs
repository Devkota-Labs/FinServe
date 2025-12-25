using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Interfaces;
using Serilog;
using Shared.Application.Api;
using Shared.Common;

namespace Notification.Api.Controllers;

[ApiVersion("1.0")]
[Authorize]
public sealed class NotificationsController(ILogger logger
    , IUserNotificationService userNotificationService)
    : BaseApiController(logger.ForContext<NotificationsController>())
{
    [HttpGet]
    public async Task<IActionResult> GetMyNotifications(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var serviceResponse = await userNotificationService.GetByUserIdAsync(userId, 20, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var serviceResponse = await userNotificationService.GetUnreadCountAsync(userId, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("read/{id}")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await userNotificationService.MarkAsReadAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    // Helper method
    private int GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        return claim != null ? int.Parse(claim.Value, Constants.IFormatProvider) : 0;
    }
}