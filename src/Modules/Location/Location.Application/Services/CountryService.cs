//using Location.Application.Interfaces.Repositories;
//using Location.Domain;

//namespace Location.Application.Services
//{
//    public class CountryService
//    {
//        private readonly ICountryRepository _countries;

//        public CountryService(ICountryRepository countries)
//        {
//            _countries = countries;
//        }

//        public Task<List<Country>> GetAllAsync()
//            => _countries.GetAllAsync();

//        public Task<Country?> GetByIdAsync(int id)
//            => _countries.GetByIdAsync(id);

//        public Task<Country?> GetByName(int id)
//            => _countries.GetByIdAsync(id);

//        public async Task AddAsync(string name, string iso, string code)
//        {
//            var country = new Country
//            {
//                Name = name,
//                IsoCode = iso,
//                MobileCode = code,
//                CreatedTime = DateTime.UtcNow,
//                LastUpdatedTime = DateTime.UtcNow
//            };

//            await _countries.AddAsync(country);
//        }

//        public Task<bool> ExistsAsync(int id)
//            => _countries.ExistsAsync(id);
//    }
//}
