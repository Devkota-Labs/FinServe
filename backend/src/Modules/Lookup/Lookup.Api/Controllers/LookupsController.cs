using Asp.Versioning;
using Lookup.Application.Lookups;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Application.Api;

namespace Lookup.Api.Controllers;

[ApiVersion("1.0")]
public sealed class LookupsController(
    ILogger logger
    , GenderLookup genderLookup
    , AddressTypeLookup addressTypeLookup
    )
    : BaseApiController(logger.ForContext<LookupsController>())
{
    [HttpGet("genders")]
    public IActionResult GetGenders()
    {
        var lookupResult = genderLookup.Get();

        return FromResult(lookupResult);
    }

    [HttpGet("addressTypes")]
    public IActionResult GetAddressTypes()
    {
        var lookupResult = addressTypeLookup.Get();

        return FromResult(lookupResult);
    }
}