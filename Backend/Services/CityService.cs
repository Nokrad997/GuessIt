using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Repositories;

namespace Backend.Services;

public class CityService
{
    private readonly CityRepository _cityRepository;
    private readonly GeolocationRepository _geolocationRepository;
    
    public CityService(CityRepository cityRepository, GeolocationRepository geolocationRepository)
    {
        _cityRepository = cityRepository;
        _geolocationRepository = geolocationRepository;
    }
    
    public async Task<CityDto> Retrieve(int cityId)
    {
        var city = await _cityRepository.GetCityById(cityId);
        if (city is null)
        {
            throw new ArgumentException("City with provided id not found");
        }
        
        return city.ConvertToDto();
    }
    
    public async Task<IEnumerable<CityDto>> RetrieveByCountryId(int countryId)
    {
        var cities = await _cityRepository.GetCitiesByCountryId(countryId);
        
        return cities.Select(c => c.ConvertToDto()).ToList();
    }
    
    public async Task<IEnumerable<CityDto>> Retrieve()
    {
        var cities = await _cityRepository.GetCities();
        return cities.Select(c => c.ConvertToDto());
    }
    
    public async Task AddCity(CityDto cityDto)
    {
        var city = await _cityRepository.GetCityByGeolocationId(cityDto.GeolocationIdFk);
        if (city is not null)
        {
            throw new ArgumentException("City with provided geolocation id already exists");
        }
        if(await _cityRepository.GetCityByName(cityDto.CityName) is not null)
        {
            throw new ArgumentException("Continent with provided name already exists");
        }
        if(await _geolocationRepository.GetGeolocationById(cityDto.GeolocationIdFk) is null)
        {
            throw new ArgumentException("Geolocation with provided id not found");
        }
        
        await _cityRepository.AddCity(cityDto.ConvertToEntity());
    }
    
    public async Task<CityDto> EditCity(int cityId, EditCityDto editCityDto)
    {
        var city = await _cityRepository.GetCityById(cityId);
        if (city is null)
        {
            throw new ArgumentException("City with provided id not found");
        }
        if(await _cityRepository.GetCityByName(editCityDto.CityName) is not null)
        {
            throw new ArgumentException("Continent with provided name already exists");
        }
        if(await _geolocationRepository.GetGeolocationById(editCityDto.GeolocationIdFk) is null)
        {
            throw new ArgumentException("Geolocation with provided id not found");
        }
        
        city.CityName = editCityDto.CityName;
        city.GeolocationIdFk = editCityDto.GeolocationIdFk;
        
        await _cityRepository.EditCity(city);
        
        return city.ConvertToDto();
    }
    
    public async Task DeleteCity(int cityId)
    {
        var city = await _cityRepository.GetCityById(cityId);
        if (city is null)
        {
            throw new ArgumentException("City with provided id not found");
        }
        
        await _cityRepository.DeleteCity(city);
    }
}