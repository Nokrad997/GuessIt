using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        
        public int CountryIdFk { get; set; }
        public Country Country { get; set; }
        
        public int GeolocationIdFk { get; set; }
        public Geolocation Geolocation { get; set; }
}