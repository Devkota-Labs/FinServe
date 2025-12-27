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
    , INotificationQueryService notificationQueryService)
    : BaseApiController(logger.ForContext<NotificationsController>())
{
    [HttpGet]
    public async Task<IActionResult> GetMyNotifications([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var userId = GetCurrentUserId();

        var serviceResponse = await notificationQueryService.GetByUserIdAsync(userId, page, pageSize, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var serviceResponse = await notificationQueryService.GetUnreadCountAsync(userId, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    [HttpPatch("read/{id}")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken)
    {
        var serviceResponse = await notificationQueryService.MarkAsReadAsync(id, cancellationToken).ConfigureAwait(false);

        return FromResult(serviceResponse);
    }

    // Helper method
    private int GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        return claim != null ? int.Parse(claim.Value, Constants.IFormatProvider) : 0;
    }
}