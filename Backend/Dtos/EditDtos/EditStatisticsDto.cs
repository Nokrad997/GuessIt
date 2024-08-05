using System.ComponentModel.DataAnnotations;
using Backend.Entities;

namespace Backend.Dtos.EditDtos;

public class EditStatisticsDto : IValidatableObject
{
    public int UserIdFk { get; set; }
    public int TotalGames { get; set; }
    public int TotalPoints { get; set; }
    public int HighestScore { get; set; }
    public double LowestTimeInSeconds { get; set; }
    public double TotalTraveledDistanceInMeters { get; set; }
    public double AverageScore { get; set; }

    public Statistics ConvertToEntity()
    {
        return new Statistics
        {
            UserIdFk = UserIdFk,
            TotalGames = TotalGames,
            TotalPoints = TotalPoints,
            HighestScore = HighestScore,
            LowestTimeInSeconds = LowestTimeInSeconds,
            TotalTraveledDistanceInMeters = TotalTraveledDistanceInMeters,
            AverageScore = AverageScore
        };
    }

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