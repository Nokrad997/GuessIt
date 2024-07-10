using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Backend.Entities;

[Table("geolocations")]
public class Geolocations
{
    [Key]
    [Required]
    [Column("geolocation_id")]
    public int GeolocationId { get; set; }

    [Required]
    [Column("area")]
    public Polygon Area { get; set; }
}
