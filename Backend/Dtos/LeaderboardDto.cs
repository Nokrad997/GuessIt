using System.ComponentModel.DataAnnotations;
using Backend.Dtos.EditDtos;
using Backend.Entities;

namespace Backend.Dtos.Interfaces;

public class LeaderboardDto : EditLeaderboardDto, IValidatableObject
{
    public int LeaderBoardId { get; init; }
    public int UserIdFk { get; init; }
    
    public new Leaderboard ConvertToEntity()
    {
        return new Leaderboard
        {
            LeaderBoardId = LeaderBoardId,
            UserIdFk = UserIdFk,
            TotalPoints = TotalPoints
        };
    }
    
    public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
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