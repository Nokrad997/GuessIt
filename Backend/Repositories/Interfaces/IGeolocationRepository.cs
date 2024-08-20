using Backend.Entities;
using NetTopologySuite.Geometries;

namespace Backend.Repositories.Interfaces;

public interface IGeolocationRepository
{
    Task<IEnumerable<Geolocation>> GetGeolocations();
    Task<Geolocation?> GetGeolocationById(int id);
    Task<Geolocation?> GetGeolocationByArea(Polygon area);
    Task AddGeolocation(Geolocation geolocation);
    Task EditGeolocation(Geolocation geolocation);
    Task DeleteGeolocation(Geolocation geolocation);
}