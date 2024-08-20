using Backend.Context;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Backend.Repositories;

public class GeolocationRepository : IGeolocationRepository
{
    private readonly GuessItContext _context;
    
    public GeolocationRepository(GuessItContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Geolocation>> GetGeolocations()
    {
        return await _context.Geolocation.ToListAsync();
    }

    public async Task<Geolocation?> GetGeolocationById(int id)
    {
        return await _context.Geolocation.FirstOrDefaultAsync(g => g.GeolocationId == id);
    }

    public async Task<Geolocation?> GetGeolocationByArea(Polygon area)
    {
        return await _context.Geolocation.FirstOrDefaultAsync(g => g.Area == area);
    }

    public async Task AddGeolocation(Geolocation geolocation)
    {
        await _context.Geolocation.AddAsync(geolocation);
        await SaveChanges();
    }

    public async Task EditGeolocation(Geolocation geolocation)
    {
        _context.Geolocation.Update(geolocation);
        await SaveChanges();
    }

    public async Task DeleteGeolocation(Geolocation geolocation)
    {
        _context.Geolocation.Remove(geolocation);
        await SaveChanges();
    }
    
    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}