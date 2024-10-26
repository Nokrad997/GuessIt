using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services;

public class GeolocationService : IGeolocationService
{
    private readonly IGeolocationRepository _geolocationRepository;
    
    public GeolocationService(IGeolocationRepository geolocationRepository)
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
    
    public async Task AddGeolocation(GeolocationDto geolocationDto)
    {
        var geolocation = await _geolocationRepository.GetGeolocationByArea(geolocationDto.Area);
        if (geolocation is not null)
        {
            throw new ArgumentException("Geolocation with provided area already exists");
        }
        
        await _geolocationRepository.AddGeolocation(geolocationDto.ConvertToEntity());
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
    
    public async Task DeleteGeolocation(int geolocationId)
    {
        var geolocation = await _geolocationRepository.GetGeolocationById(geolocationId);
        if (geolocation is null)
        {
            throw new ArgumentException("Geolocation with provided id not found");
        }
        
        await _geolocationRepository.DeleteGeolocation(geolocation);
    }
}