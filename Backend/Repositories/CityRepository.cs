using Backend.Context;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class CityRepository : ICityRepository
{
    private readonly GuessItContext _context;
    
    public CityRepository(GuessItContext context)
    {
        _context = context;
    }
    
    public async Task<City?> GetCityById(int cityId)
    {
        return await _context.City.FirstOrDefaultAsync(c => c.CityId == cityId);    
    }

    public async Task<IEnumerable<City>> GetCitys()
    {
        return await _context.City.ToListAsync();
    }
    
    public async Task<City?> GetCityByGeolocationId(int geolocationId)
    {
        return await _context.City.FirstOrDefaultAsync(c => c.GeolocationIdFk == geolocationId);
    }

    public async Task AddCity(City city)
    {
        await _context.City.AddAsync(city);
        await SaveChanges();
    }

    public async Task EditCity(City city)
    {
        _context.City.Update(city);
        await SaveChanges();
    }

    public async Task DeleteCity(City city)
    {
        _context.City.Remove(city);
        await SaveChanges();
    }
    
    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}