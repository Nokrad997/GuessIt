using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Dtos;
using Backend.Entities.Interfaces;

namespace Backend.Entities;

[Table("achievements")]
public class Achievement : IHasTimeStamp
{
    [Key]
    [Required]
    [Column("achievement_id")]
    public int AchievementId { get; set; }
    
    [Required]
    [Column("achievement_name")]
    public string AchievementName { get; set; }
    
    [Required]
    [Column("achievement_description")]
    public string AchievementDescription { get; set; }


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