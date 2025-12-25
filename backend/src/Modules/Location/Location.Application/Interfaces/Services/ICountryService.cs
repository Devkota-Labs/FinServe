using Location.Application.Dtos;
using Shared.Application.Interfaces.Services;

namespace Location.Application.Interfaces.Services;

public interface ICountryService : IService<CountryDto, CreateCountryDto, UpdateCountryDto>
{
}
