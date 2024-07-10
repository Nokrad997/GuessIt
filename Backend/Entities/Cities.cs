using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;
    [Table("cities")]
    public class Cities
    {
        [Key]
        [Required]
        [Column("city_id")]
        public int CityId { get; set; }

        [Required]
        [Column("city_name")]
        public string CityName { get; set; }

        [Required]
        [ForeignKey("Countries")]
        [Column("country_id")]
        public int CountryId { get; set; }

        [Required]
        [ForeignKey("GeolocationId")]
        [Column("geolocation_id")]
        public Geolocations GeolocationId { get; set; }
}