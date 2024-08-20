using Backend.Dtos;
using NetTopologySuite.Geometries;

namespace Backend.Entities;

public class Geolocation
{
    public int GeolocationId { get; set; }
    public Polygon Area { get; set; }
    
    public ICollection<City> Cities { get; set; }
    public ICollection<Country> Countries { get; set; }
    public ICollection<Continent> Continents { get; set; }
    
    public GeolocationDto ConvertToDto()
    {
        return new GeolocationDto()
        {
            GeolocationId = GeolocationId,
            Area = Area
        };
    }
}
