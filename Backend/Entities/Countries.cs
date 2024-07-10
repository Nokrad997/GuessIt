using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;

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
    [ForeignKey("ContinentId")]
    [Column("continent_id")]
    public Continents ContinentId { get; set; }

    [Required]
    [ForeignKey("GeolocationId")]
    [Column("geolocation_id")]
    public Geolocations GeolocationId { get; set; }

    public ICollection<Cities> Cities { get; set; }
}