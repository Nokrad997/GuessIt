using Backend.Dtos;
using Backend.Dtos.EditDtos;

namespace Backend.Services.Interfaces;

public interface ICountryService
{
    Task<CountryDto> Retrieve(int countryId);
    Task<IEnumerable<CountryDto>> RetrieveByContinentId(int continentId);
    Task<IEnumerable<CountryDto>> Retrieve();
    Task AddCountry(CountryDto countryDto);
    Task<CountryDto> EditCountry(int countryId, EditCountryDto editCountryDto);
    Task DeleteCountry(int countryId);
}