using Backend.Entities;
using System.Text.Json.Serialization;
namespace Backend.Dtos;

public class AchievementDto
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
}