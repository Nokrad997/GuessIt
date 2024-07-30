using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Dtos;
using Backend.Entities.Interfaces;

namespace Backend.Entities;

[Table("achievements")]
public class Achievement : IHasTimeStamp
{
    public int AchievementId { get; set; }
    
    public string AchievementName { get; set; }

    public string AchievementDescription { get; set; }

    public ICollection<UserAchievements> UserAchievements { get; set; }

    public AchievementDto ConvertToDto()
    {
        return new AchievementDto
        {
            AchievementId = AchievementId,
            AchievementName = AchievementName,
            AchievementDescription = AchievementDescription
        };
    }
}