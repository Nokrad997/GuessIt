using System.ComponentModel.DataAnnotations;
using Backend.Entities;

namespace Backend.Dtos.EditDtos;

public class EditLeaderboardDto : IValidatableObject
{
    public int TotalPoints { get; set; }
    
    public Leaderboard ConvertToEntity()
    {
        return new Leaderboard
        {
            TotalPoints = TotalPoints
        };
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TotalPoints < 0)
        {
            yield return new ValidationResult("TotalPoints must be greater than or equal to 0", new[] {nameof(TotalPoints)});
        }
    }
}