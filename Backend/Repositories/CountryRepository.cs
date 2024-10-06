using Backend.Context;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly GuessItContext _context;
    
    public CountryRepository(GuessItContext context)
    {
        _context = context;
    }
    
    public async Task<Country?> GetCountryById(int countryId)
    {
        return await _context.Country.FirstOrDefaultAsync(c => c.CountryId == countryId);    
    }

    public async Task<IEnumerable<Country>> GetCountriesByContinentId(int continentId)
    {
        return await _context.Country.Where(c => c.ContinentIdFk == continentId).ToListAsync();    
    }
    
    public async Task<IEnumerable<Country>> GetCountries()
    {
        return await _context.Country.ToListAsync();
    }
    
    public async Task<Country?> GetCountryByGeolocationId(int geolocationId)
    {
        return await _context.Country.FirstOrDefaultAsync(c => c.GeolocationIdFk == geolocationId);
    }

    public async Task<Country?> GetCountryByName(string countryName)
    {
        return await _context.Country.FirstOrDefaultAsync(c => c.CountryName == countryName);
    }
    
    public async Task AddCountry(Country country)
    {
        await _context.Country.AddAsync(country);
        await SaveChanges();
    }

    public async Task EditCountry(Country country)
    {
        _context.Country.Update(country);
        await SaveChanges();
    }

    public async Task DeleteCountry(Country country)
    {
        _context.Country.Remove(country);
        await SaveChanges();
    }
    
    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}