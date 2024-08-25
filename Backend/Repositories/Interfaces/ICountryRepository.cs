using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface ICountryRepository
{
    Task<Country?> GetCountryById(int countryId); 
    Task<IEnumerable<Country>> GetCountries();
    Task AddCountry(Country country);
    Task EditCountry(Country country);
    Task DeleteCountry(Country country);
}