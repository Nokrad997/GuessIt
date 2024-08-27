using System.ComponentModel.DataAnnotations;
using Backend.Entities;

namespace Backend.Dtos.EditDtos;

public class EditStatisticsDto : IValidatableObject
{
    public int UserIdFk { get; init; }
    public int TotalGames { get; init; }
    public int TotalPoints { get; init; }
    public int HighestScore { get; init; }
    public double LowestTimeInSeconds { get; init; }
    public double TotalTraveledDistanceInMeters { get; init; }
    public double AverageScore { get; init; }
    

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(TotalGames == 0 && TotalPoints == 0 && HighestScore == 0 && LowestTimeInSeconds == 0 && TotalTraveledDistanceInMeters == 0 && AverageScore == 0)
        {
            yield return new ValidationResult("No changes detected",
                new[]
                {
                    nameof(TotalGames), nameof(TotalPoints), nameof(HighestScore), nameof(LowestTimeInSeconds),
                    nameof(TotalTraveledDistanceInMeters), nameof(AverageScore)
                });
        }
        if (TotalGames < 0)
        {
            yield return new ValidationResult("Total games cannot be negative",
                new[] {nameof(TotalGames)});
        }
        if(TotalPoints < 0)
        {
            yield return new ValidationResult("Total points cannot be negative",
                new[] {nameof(TotalPoints)});
        }
        if(HighestScore < 0)
        {
            yield return new ValidationResult("Highest score cannot be negative",
                new[] {nameof(HighestScore)});
        }
        if(LowestTimeInSeconds < 0)
        {
            yield return new ValidationResult("Lowest time cannot be negative",
                new[] {nameof(LowestTimeInSeconds)});
        }
        if(TotalTraveledDistanceInMeters < 0)
        {
            yield return new ValidationResult("Total distance cannot be negative",
                new[] {nameof(TotalTraveledDistanceInMeters)});
        }
        if(AverageScore < 0)
        {
            yield return new ValidationResult("Average score cannot be negative",
                new[] {nameof(AverageScore)});
        }
    }
}