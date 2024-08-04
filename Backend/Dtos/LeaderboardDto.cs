using System.ComponentModel.DataAnnotations;
using Backend.Entities;

namespace Backend.Dtos.Interfaces;

public class LeaderboardDto : IValidatableObject
{
    public int LeaderBoardId { get; set; }
    public int UserIdFk { get; set; }
    public int TotalPoints { get; set; }
    
    public Leaderboard ConvertToEntity()
    {
        return new Leaderboard
        {
            LeaderBoardId = LeaderBoardId,
            UserIdFk = UserIdFk,
            TotalPoints = TotalPoints
        };
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(UserIdFk < 0)
        {
            yield return new ValidationResult("UserIdFk must be greater than or equal to 0", new[] {nameof(UserIdFk)});
        }
        if (TotalPoints < 0)
        {
            yield return new ValidationResult("TotalPoints must be greater than or equal to 0", new[] {nameof(TotalPoints)});
        }
    }
}