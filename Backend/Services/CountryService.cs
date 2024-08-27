using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Repositories;

namespace Backend.Services;

public class CountryService
{
     private readonly CountryRepository _countryRepository;
    
    public CountryService(CountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }
    
    public async Task<CountryDto> Retrieve(int countryId)
    {
        var country = await _countryRepository.GetCountryById(countryId);
        if (country == null)
        {
            throw new ArgumentException("Country with provided id not found");
        }
        
        return country.ConvertToDto();
    }
    
    public async Task<IEnumerable<CountryDto>> Retrieve()
    {
        var countries = await _countryRepository.GetCountries();
        return countries.Select(c => c.ConvertToDto());
    }
    
    public async Task AddCountry(CountryDto countryDto)
    {
        var country = await _countryRepository.GetCountryByGeolocationId(countryDto.GeolocationIdFk);
        if (country != null)
        {
            throw new ArgumentException("Country with provided geolocation id already exists");
        }
        
        await _countryRepository.AddCountry(countryDto.ConvertToEntity());
    }
    
    public async Task<CountryDto> EditCountry(int countryId, EditCountryDto editCountryDto)
    {
        var country = await _countryRepository.GetCountryById(countryId);
        if (country == null)
        {
            throw new ArgumentException("Country with provided id not found");
        }
        
        country.CountryName = editCountryDto.CountryName;
        country.GeolocationIdFk = editCountryDto.GeolocationIdFk;
        
        await _countryRepository.EditCountry(country);
        
        return country.ConvertToDto();
    }
    
    public async Task DeleteCountry(int countryId)
    {
        var country = await _countryRepository.GetCountryById(countryId);
        if (country == null)
        {
            throw new ArgumentException("Country with provided id not found");
        }
        
        await _countryRepository.DeleteCountry(country);
    }
}