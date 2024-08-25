using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface IContinentRepository
{
    Task<Continent?> GetContinentById(int continentId); 
    Task<IEnumerable<Continent>> GetContinents();
    Task AddContinent(Continent continent);
    Task EditContinent(Continent continent);
    Task DeleteContinent(Continent continent);
}