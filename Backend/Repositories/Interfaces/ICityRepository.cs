using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface ICityRepository
{
    Task<City?> GetCityById(int cityId); 
    Task<IEnumerable<City>> GetCities();
    Task<City?> GetCityByGeolocationId(int geolocationId);
    Task<City?> GetCityByName(string cityName);
    Task AddCity(City city);
    Task EditCity(City city);
    Task DeleteCity(City city);
}