using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface IContinentRepository
{
    Task<Continent?> GetContinentById(int continentId); 
    Task<IEnumerable<Continent>> GetContinents();
    Task<Continent?> GetContinentByGeolocationId(int geolocationId);
    Task<Continent?> GetContinentByName(string continentName);
    Task AddContinent(Continent continent);
    Task EditContinent(Continent continent);
    Task DeleteContinent(Continent continent);
}