using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;
    [Table("cities")]
    public class City
    {
        [Key]
        [Required]
        [Column("city_id")]
        public int CityId { get; set; }

        [Required]
        [Column("city_name")]
        public string CityName { get; set; }

        [Required]
        [ForeignKey("CountryIdFk")]
        [Column("country_id")]
        public int CountryIdFk { get; set; }
        
        public Country Country { get; set; }

        [Required]
        [ForeignKey("GeolocationIdFk")]
        [Column("geolocation_id")]
        public int GeolocationIdFk { get; set; }
        
        public Geolocation Geolocation { get; set; }
}