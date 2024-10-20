using System.ComponentModel.DataAnnotations;
using Backend.Entities;
using Newtonsoft.Json;
using static System.String;

namespace Backend.Dtos;

public class EditAchievementDto : IValidatableObject
{
    public string AchievementName { get; set; }
    public string AchievementDescription { get; set; }
    public Dictionary<string, object> AchievementCriteria { get; set; }

    public Achievement ConvertToEntity()
    {
        return new Achievement
        {
            AchievementName = AchievementName,
            AchievementDescription = AchievementDescription,
            AchievementCriteria = AchievementCriteria
        };
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsNullOrEmpty(AchievementName) && IsNullOrEmpty(AchievementDescription))
        {
            yield return new ValidationResult("No changes detected", new[] { nameof(AchievementName), nameof(AchievementDescription) });
        }
    }
}