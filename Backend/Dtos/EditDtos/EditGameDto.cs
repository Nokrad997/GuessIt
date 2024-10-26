using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend.Entities;
using NetTopologySuite.Geometries;

namespace Backend.Dtos.EditDtos;

public class EditGameDto : IValidatableObject
{
    public int UserIdFk { get; set; }
    
    [JsonConverter(typeof(PointConverter))]
    public Point StartLocation { get; set; }
    
    [JsonConverter(typeof(PointConverter))]
    public Point GuessedLocation { get; set; }
    public double DistanceToStartingLocation { get; set; }
    public double TraveledDistance { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Score { get; set; }

    public Game ConvertToEntity()
    {
        return new Game
        { 
            UserIdFk = UserIdFk,
            StartLocation = StartLocation,
            GuessedLocation = GuessedLocation,
            DistanceToStartingLocation = DistanceToStartingLocation,
            TraveledDistance = TraveledDistance,
            StartTime = StartTime,
            EndTime = EndTime,
            Score = Score
        };
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserIdFk < 0)
        {
            yield return new ValidationResult("UserIdFk must be greater than 0", new[] { nameof(UserIdFk) });
        }
        if (StartLocation == null)
        {
            yield return new ValidationResult("StartLocation is required", new[] { nameof(StartLocation) });
        }
        if (GuessedLocation == null)
        {
            yield return new ValidationResult("GuessedLocation is required", new[] { nameof(GuessedLocation) });
        }
        if (DistanceToStartingLocation < 0)
        {
            yield return new ValidationResult("DistanceToStartingLocation must be greater than or equal to 0", new[] { nameof(DistanceToStartingLocation) });
        }
        if (StartTime == null)
        {
            yield return new ValidationResult("StartTime must be a valid non-zero time", new[] { nameof(StartTime) });
        }
        if (EndTime == null)
        {
            yield return new ValidationResult("EndTime must be a valid non-zero time", new[] { nameof(EndTime) });
        }
        if (Score < 0)
        {
            yield return new ValidationResult("Score must be greater than or equal to 0", new[] { nameof(Score) });
        }
    }
}