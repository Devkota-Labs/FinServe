using Microsoft.AspNetCore.Http;
using Shared.Application.Dtos;

namespace Shared.Application.Interfaces.Services;

public interface IDeviceResolver
{
    DeviceInfo Resolve(HttpContext? context);
}
