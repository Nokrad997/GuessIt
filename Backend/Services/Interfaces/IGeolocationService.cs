using Backend.Dtos;
using Backend.Dtos.EditDtos;

namespace Backend.Services.Interfaces;

public interface IGeolocationService
{
    Task<GeolocationDto> Retrieve(int geolocationId);
    Task<IEnumerable<GeolocationDto>> Retrieve();
    Task AddGeolocation(GeolocationDto geolocationDto);
    Task<GeolocationDto> EditGeolocation(int geolocationId, EditGeolocationDto editGeolocationDto);
    Task DeleteGeolocation(int geolocationId);
}