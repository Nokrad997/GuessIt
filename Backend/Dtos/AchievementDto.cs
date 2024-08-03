using System.ComponentModel.DataAnnotations;
using Backend.Entities;
using static System.String;

namespace Backend.Dtos;

public class AchievementDto : IValidatableObject
{
    public int AchievementId { get; set; }
    public string AchievementName { get; set; }
    public string AchievementDescription { get; set; }

    public Achievement ConvertToEntity()
    {
        return new Achievement
        {
            AchievementId = AchievementId,
            AchievementName = AchievementName,
            AchievementDescription = AchievementDescription
        };
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
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