using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Entities.Interfaces;

namespace Backend.Entities;

[Table("user_achievements")]
public class UserAchievements : IHasTimeStamp
{
    [Key]
    [Required]
    [Column("user_achievement_id")]
    public int UserAchievementId { get; set; }
    
    [Required]
    [ForeignKey("UserIdFk")]
    [Column("user_id")]
    public int UserIdFk { get; set; }
    
    public User User { get; set; }
    
    [Required]
    [ForeignKey("AchievementIdFk")]
    [Column("achievement_id")]
    public int AchievementId { get; set; }
    
    public Achievement Achievement { get; set; }
}