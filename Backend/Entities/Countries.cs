using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

[Table("countries")]
public class Countries
{
    [Key]
    [Required]
    [Column("country_id")]
    public int CountryId { get; set; }

    [Required]
    [Column("country_name")] 
    public string CountryName { get; set; }

    [Required]
    [ForeignKey("ContinentIdFk")]
    [Column("continent_id")]
    public int ContinentIdFk { get; set; }

    public Continents Continent { get; set; }
    
    [Required]
    [ForeignKey("GeolocationIdFk")]
    [Column("geolocation_id")]
    public int GeolocationId { get; set; }

    public Geolocations Geolocation { get; set; }
    
    public ICollection<Cities> Cities { get; set; }
}