using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface ICityRepository
{
    Task<City?> GetCityById(int cityId); 
    Task<IEnumerable<City>> GetCitys();
    Task AddCity(City city);
    Task EditCity(City city);
    Task DeleteCity(City city);
}