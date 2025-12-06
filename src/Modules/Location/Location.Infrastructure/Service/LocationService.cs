//using Location.Application.Dtos;
//using Location.Application.Interfaces.Repositories;
//using Location.Application.Interfaces.Services;
//using Location.Domain;

//namespace Location.Infrastructure.Service
//{
//    internal class LocationService : ILocationService
//    {
//        private readonly ICountryRepository _countries;
//        private readonly IStateRepository _states;
//        private readonly ICityRepository _cities;

//        public LocationService(ICountryRepository countries, IStateRepository states, ICityRepository cities)
//        {
//            _countries = countries;
//            _states = states;
//            _cities = cities;
//        }

//        private CountryDto MapToCountryDto(Country country)
//        {
//            return new CountryDto(country.Id, country.Name, country.IsoCode, country.MobileCode);
//        }

//        private StateDto MapToStateDto(State state)
//        {
//            return new StateDto(state.Id, state.Name, state.CountryId);
//        }

//        private CityDto MapToCityDto(City city)
//        {
//            return new CityDto(city.Id, city.Name, city.StateId);
//        }

//        public async Task<List<CountryDto>> GetCountriesAsync()
//        {
//            var list = await _countries.GetAllAsync().ConfigureAwait(false);

//            return [.. list.Select(MapToCountryDto)];
//        }

//        public async Task<CountryDto?> GetCountryAsync(int id)
//        {
//            var item = await _countries.GetByIdAsync(id).ConfigureAwait(false);
//            if (item == null)
//                return null;
//            return MapToCountryDto(item);
//        }

//        public async Task<CountryDto> CreateCountryAsync(CreateCountryDto dto)
//        {
//            var country = new Country
//            {
//                Name = dto.Name,
//                IsoCode = dto.IsoCode,
//                MobileCode = dto.MobileCode
//            };

//            await _countries.AddAsync(country).ConfigureAwait(false);
//            await _countries.SaveAsync();

//            return MapToCountryDto(country);
//        }

//        public async Task UpdateCountryAsync(int id, UpdateCountryDto dto)
//        {
//            var item = await _countries.GetByIdAsync(id).ConfigureAwait(false);

//            if (item == null)
//                throw new Exception("Country not found");

//            item.Name = dto.Name;
//            await _countries.UpdateAsync(item).ConfigureAwait(false);
//            await _countries.SaveAsync();
//        }

//        public async Task DeleteCountryAsync(int id)
//        {
//            var item = await _countries.GetByIdAsync(id).ConfigureAwait(false);
//            if (item == null) throw new Exception("Country not found");

//            await _countries.DeleteAsync(item).ConfigureAwait(false);
//            await _countries.SaveAsync();
//        }

//        public async Task<List<StateDto>> GetStatesByCountryAsync(int countryId)
//        {
//            var list = await _states.GetByCountryAsync(countryId).ConfigureAwait(false);

//            return [.. list.Select(MapToStateDto)];
//        }

//        //public async Task<List<StateDto>> GetStatesAsync()
//        //{
//        //    var list = await _countries.GetAllAsync();
//        //    return list.Select(x => new StateDto(x.Id, x.Name)).ToList();
//        //}

//        //public async Task<StateDto?> GetStateAsync(int id)
//        //{
//        //    var item = await _countries.GetByIdAsync(id);
//        //    if (item == null) return null;
//        //    return new StateDto { Id = item.Id, Name = item.Name };
//        //}

//        //public async Task<StateDto> CreateStateAsync(CreateStateDto dto)
//        //{
//        //    var state = new State { Name = dto.Name };
//        //    await _countries.AddAsync(state);
//        //    await _countries.SaveAsync();
//        //    return new StateDto { Id = state.Id, Name = state.Name };
//        //}

//        //public async Task UpdateStateAsync(int id, UpdateStateDto dto)
//        //{
//        //    var item = await _countries.GetByIdAsync(id);
//        //    if (item == null) throw new Exception("State not found");

//        //    item.Name = dto.Name;
//        //    await _countries.UpdateAsync(item);
//        //    await _countries.SaveAsync();
//        //}

//        //public async Task DeleteStateAsync(int id)
//        //{
//        //    var item = await _countries.GetByIdAsync(id);
//        //    if (item == null) throw new Exception("State not found");

//        //    await _countries.DeleteAsync(item);
//        //    await _countries.SaveAsync();
//        //}

//        public async Task<List<CityDto>> GetCitiesByStateAsync(int stateId)
//        {
//            var list = await _cities.GetByStateAsync(stateId).ConfigureAwait(false);

//            return [.. list.Select(MapToCityDto)];
//        }
//    }
//}
