//using Location.Application.Dtos;

//namespace Location.Application.Interfaces.Services
//{
//    public interface ILocationService
//    {
//        // Country
//        Task<List<CountryDto>> GetCountriesAsync();
//        Task<CountryDto?> GetCountryAsync(int id);
//        Task<CountryDto> CreateCountryAsync(CreateCountryDto dto);
//        Task UpdateCountryAsync(int id, UpdateCountryDto dto);
//        Task DeleteCountryAsync(int id);

//        // State
//        Task<List<StateDto>> GetStatesByCountryAsync(int countryId);
//        //Task<List<StateDto>> GetStatesAsync();
//        //Task<StateDto?> GetStateAsync(int id);
//        //Task<StateDto> CreateStateAsync(CreateStateDto dto);
//        //Task UpdateStateAsync(int id, UpdateStateDto dto);
//        //Task DeleteStateAsync(int id);

//        // City
//        Task<List<CityDto>> GetCitiesByStateAsync(int stateId);
//        //Task<List<CityDto>> GetCitysAsync();
//        //Task<CityDto?> GetCityAsync(int id);
//        //Task<CityDto> CreateCityAsync(CreateCityDto dto);
//        //Task UpdateCityAsync(int id, UpdateCityDto dto);
//        //Task DeleteCityAsync(int id);
//    }
//}
