using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

public class Country
{
    public int CountryId { get; set; }
    public string CountryName { get; set; }
    
    public int ContinentIdFk { get; set; }
    public Continent Continent { get; set; }
    
    public int GeolocationIdFk { get; set; }
    public Geolocation Geolocation { get; set; }
    
    public ICollection<City> Cities { get; set; }
}