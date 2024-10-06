using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend.Entities;
using NetTopologySuite.Geometries;

namespace Backend.Dtos.EditDtos;

public class EditGeolocationDto : IValidatableObject
{
    [JsonConverter(typeof(PolygonConverter))]
    public Geometry Area { get; set; }
    
    public Geolocation ConvertToEntity()
    {
        return new Geolocation
        {
            Area = Area
        };
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Area == null)
        {
            yield return new ValidationResult("Area is required", new[] {nameof(Area)});
        }
    }
}