using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Repositories;

namespace Backend.Services;

public class GeolocationService
{
    private readonly GeolocationRepository _geolocationRepository;
    
    public GeolocationService(GeolocationRepository geolocationRepository)
    {
        _geolocationRepository = geolocationRepository;
    }
    
    public async Task<GeolocationDto> Retrieve(int geolocationId)
    {
        var geolocation = await _geolocationRepository.GetGeolocationById(geolocationId);
        if (geolocation is null)
        {
            throw new ArgumentException("Geolocation with provided id not found");
        }
        
        return geolocation.ConvertToDto();
    }
    
    public async Task<IEnumerable<GeolocationDto>> Retrieve()
    {
        var geolocations = await _geolocationRepository.GetGeolocations();
        
        return geolocations.Select(g => g.ConvertToDto());
    }
    
    public async Task<GeolocationDto> CreateGeolocation(GeolocationDto geolocationDto)
    {
        var geolocation = await _geolocationRepository.GetGeolocationByArea(geolocationDto.Area);
        if (geolocation != null)
        {
            throw new ArgumentException("Geolocation with provided area already exists");
        }
        
        await _geolocationRepository.AddGeolocation(geolocationDto.ConvertToEntity());
        
        return geolocationDto;
    }
    
    public async Task<GeolocationDto> EditGeolocation(int geolocationId, EditGeolocationDto editGeolocationDto)
    {
        var geolocation = await _geolocationRepository.GetGeolocationById(geolocationId);
        if (geolocation is null)
        {
            throw new ArgumentException("Geolocation with provided id not found");
        }
        
        geolocation.Area = editGeolocationDto.Area;
        await _geolocationRepository.EditGeolocation(geolocation);
        
        return geolocation.ConvertToDto();
    }
    
    public async Task<GeolocationDto> DeleteGeolocation(int geolocationId)
    {
        var geolocation = await _geolocationRepository.GetGeolocationById(geolocationId);
        if (geolocation is null)
        {
            throw new ArgumentException("Geolocation with provided id not found");
        }
        
        await _geolocationRepository.DeleteGeolocation(geolocation);
        
        return geolocation.ConvertToDto();
    }
}