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
            AchievementDescription = AchievementDescription,
            AchievementCriteria = AchievementCriteria
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

        if (AchievementCriteria == null || !AchievementCriteria.Any())
        {
            yield return new ValidationResult("Achievement criteria is required", new[] { nameof(AchievementCriteria) });
        }
    }
}