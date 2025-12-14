using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Shared.Application.Api;

[ApiController]
// [Route("api/v{version:apiVersion}/[area]/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController(ILogger logger) : BaseController(logger)
{
}
