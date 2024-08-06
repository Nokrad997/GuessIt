using System.ComponentModel.DataAnnotations;
using Backend.Entities;

namespace Backend.Dtos.EditDtos;

public class EditUserAchievementDtos : IValidatableObject
{
    public int UserIdFk { get; set; }
    public int AchievementIdFk { get; set; }
    
    public UserAchievements ConvertToEntity()
    {
        return new UserAchievements
        {
            UserIdFk = UserIdFk,
            AchievementIdFk = AchievementIdFk
        };
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(UserIdFk == 0)
        {
            yield return new ValidationResult("UserIdFk is required", new[] { nameof(UserIdFk) });
        }
        if(AchievementIdFk == 0)
        {
            yield return new ValidationResult("AchievementIdFk is required", new[] { nameof(AchievementIdFk) });
        }
        if (UserIdFk < 0)
        {
            yield return new ValidationResult("UserIdFk must be greater than 0", new[] { nameof(UserIdFk) });
        }
        if (AchievementIdFk < 0)
        {
            yield return new ValidationResult("AchievementIdFk must be greater than 0", new[] { nameof(AchievementIdFk) });
        }
    }
}