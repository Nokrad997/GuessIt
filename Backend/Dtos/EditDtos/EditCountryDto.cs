using System.ComponentModel.DataAnnotations;
using Backend.Entities;

namespace Backend.Dtos.EditDtos;

public class EditCountryDto : IValidatableObject
{
    public string CountryName { get; set; }
    public int ContinentIdFk { get; set; }
    public int GeolocationIdFk { get; set; }
    
    
    public Country ConvertToEntity()
    {
        return new Country
        {
            CountryName = CountryName,
            ContinentIdFk = ContinentIdFk,
            GeolocationIdFk = GeolocationIdFk
        };
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(CountryName))
        {
            yield return new ValidationResult("Country name is required", new[] { nameof(CountryName) });
        }
        if(ContinentIdFk == 0)
        {
            yield return new ValidationResult("Continent is required", new[] { nameof(ContinentIdFk) });
        }
        if(GeolocationIdFk == 0)
        {
            yield return new ValidationResult("Geolocation is required", new[] { nameof(GeolocationIdFk) });
        }
    }
}