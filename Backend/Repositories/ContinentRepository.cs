using Backend.Context;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ContinentRepository : IContinentRepository
{
    private readonly GuessItContext _context;
    
    public ContinentRepository(GuessItContext context)
    {
        _context = context;
    }
    
    public async Task<Continent?> GetContinentById(int continentId)
    {
        return await _context.Continent.FirstOrDefaultAsync(c => c.ContinentId == continentId);    
    }

    public async Task<IEnumerable<Continent>> GetContinents()
    {
        return await _context.Continent.ToListAsync();
    }
    
    public async Task<Continent?> GetContinentByGeolocationId(int geolocationId)
    {
        return await _context.Continent.FirstOrDefaultAsync(c => c.GeolocationIdFk == geolocationId);
    }
    
    public async Task<Continent?> GetContinentByName(string continentName)
    {
        return await _context.Continent.FirstOrDefaultAsync(c => c.ContinentName == continentName);
    }

    public async Task AddContinent(Continent continent)
    {
        await _context.Continent.AddAsync(continent);
        await SaveChanges();
    }

    public async Task EditContinent(Continent continent)
    {
        _context.Continent.Update(continent);
        await SaveChanges();
    }

    public async Task DeleteContinent(Continent continent)
    {
        _context.Continent.Remove(continent);
        await SaveChanges();
    }
    
    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}