using System.ComponentModel.DataAnnotations;
using Backend.Entities;

namespace Backend.Dtos.EditDtos;

public class EditCityDto : IValidatableObject
{
    public string CityName { get; set; }
        
    public int CountryIdFk { get; set; }
        
    public int GeolocationIdFk { get; set; }
    
    public City ConvertToEntity()
    {
        return new City
        {
            CityName = CityName,
            CountryIdFk = CountryIdFk,
            GeolocationIdFk = GeolocationIdFk
        };
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(CityName))
        {
            yield return new ValidationResult("City name is required", new[] {nameof(CityName)});
        }
        if (CountryIdFk <= 0)
        {
            yield return new ValidationResult("Country id is required", new[] {nameof(CountryIdFk)});
        }
        if (GeolocationIdFk <= 0)
        {
            yield return new ValidationResult("Geolocation id is required", new[] {nameof(GeolocationIdFk)});
        }
    }
}