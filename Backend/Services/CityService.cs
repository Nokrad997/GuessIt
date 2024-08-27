using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Repositories;

namespace Backend.Services;

public class CityService
{
    private readonly CityRepository _cityRepository;
    
    public CityService(CityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }
    
    public async Task<CityDto> Retrieve(int cityId)
    {
        var city = await _cityRepository.GetCityById(cityId);
        if (city == null)
        {
            throw new ArgumentException("City with provided id not found");
        }
        
        return city.ConvertToDto();
    }
    
    public async Task<IEnumerable<CityDto>> Retrieve()
    {
        var cities = await _cityRepository.GetCitys();
        return cities.Select(c => c.ConvertToDto());
    }
    
    public async Task AddCity(CityDto cityDto)
    {
        var city = await _cityRepository.GetCityByGeolocationId(cityDto.GeolocationIdFk);
        if (city != null)
        {
            throw new ArgumentException("City with provided geolocation id already exists");
        }
        
        await _cityRepository.AddCity(cityDto.ConvertToEntity());
    }
    
    public async Task<CityDto> EditCity(int cityId, EditCityDto editCityDto)
    {
        var city = await _cityRepository.GetCityById(cityId);
        if (city == null)
        {
            throw new ArgumentException("City with provided id not found");
        }
        
        city.CityName = editCityDto.CityName;
        city.GeolocationIdFk = editCityDto.GeolocationIdFk;
        
        await _cityRepository.EditCity(city);
        
        return city.ConvertToDto();
    }
    
    public async Task DeleteCity(int cityId)
    {
        var city = await _cityRepository.GetCityById(cityId);
        if (city == null)
        {
            throw new ArgumentException("City with provided id not found");
        }
        
        await _cityRepository.DeleteCity(city);
    }
}