using System.ComponentModel.DataAnnotations;
using Backend.Entities;
using static System.String;

namespace Backend.Dtos;

public class AchievementDto : EditAchievementDto, IValidatableObject
{
    public int AchievementId { get; set; }

    public new Achievement ConvertToEntity()
    {
        return new Achievement
        {
            AchievementId = AchievementId,
            AchievementName = AchievementName,
            AchievementDescription = AchievementDescription
        };
    }

    public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsNullOrEmpty(AchievementName))
        {
            yield return new ValidationResult("Achievement name is required", new[] { nameof(AchievementName) });
        }
        if (IsNullOrEmpty(AchievementDescription))
        {
            yield return new ValidationResult("Achievement description is required", new[] { nameof(AchievementDescription) });
        }
    }
}