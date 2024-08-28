using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface ICountryRepository
{
    Task<Country?> GetCountryById(int countryId); 
    Task<IEnumerable<Country>> GetCountries();
    Task<Country?> GetCountryByGeolocationId(int geolocationId);
    Task<Country?> GetCountryByName(string countryName);
    Task AddCountry(Country country);
    Task EditCountry(Country country);
    Task DeleteCountry(Country country);
}