using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;

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
    [ForeignKey("GeolocationId")]
    [Column("geolocation_id")]
    public Geolocations GeolocationId { get; set; }

    public ICollection<Countries> Countries { get; set; }
}
