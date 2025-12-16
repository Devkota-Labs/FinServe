using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Responses;
using Shared.Application.Results;

namespace Shared.Application.Api;

[ApiController]
public abstract class BaseController(ILogger logger) : ControllerBase
{
    private readonly ILogger _logger = logger;

    protected ILogger Logger => _logger;

    protected IActionResult FromResult(Result result)
    {
        return Ok(ApiResponse.FromResult(result));
    }

    protected IActionResult FromResult<T>(Result<T> result)
    {
        return Ok(ApiResponse.FromResult(result));
    }

    protected IActionResult FromPaginatedResult<T>(PaginatedResult<T> result)
    {
        if (result.Success)
            return Ok(PaginatedResponse.FromPaginatedResult(result));

        return BadRequest(PaginatedResponse.FromPaginatedResult(result));
    }
}
