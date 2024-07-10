using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

[Table("continents")]
public class Continents
{
    [Key]
    [Required]
    [Column("continent_id")]
    public int ContinentId { get; set; }

    [Required] 
    [Column("continent_name")] 
    public string ContinentName { get; set; }

    [Required]
    [ForeignKey("GeolocationIdFk")]
    [Column("geolocation_id")]
    public int GeolocationIdFk { get; set; }

    public Geolocations Geolocation { get; set; }
    
    public ICollection<Countries> Countries { get; set; }
}
