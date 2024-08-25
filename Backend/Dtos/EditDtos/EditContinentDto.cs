using System.ComponentModel.DataAnnotations;
using Backend.Entities;

namespace Backend.Dtos.EditDtos;

public class EditContinentDto : IValidatableObject
{
    public string ContinentName { get; set; }
    public int GeolocationIdFk { get; set; }

    public Continent ConvertToEntity()
    {
        return new Continent
        {
            ContinentName = ContinentName,
            GeolocationIdFk = GeolocationIdFk
        };
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(ContinentName))
        {
            yield return new ValidationResult("ContinentName is required", new[] { nameof(ContinentName) });
        }
    }
}