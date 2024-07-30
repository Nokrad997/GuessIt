using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

public class Continent
{
    public int ContinentId { get; set; }
    public string ContinentName { get; set; }

    public int GeolocationIdFk { get; set; }
    public Geolocation Geolocation { get; set; }
    
    public ICollection<Country> Countries { get; set; }
}
