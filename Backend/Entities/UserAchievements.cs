using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

[Table("user_achievements")]
public class UserAchievements
{
    [Key]
    [Required]
    [Column("user_achievement_id")]
    public int UserAchievementId { get; set; }
    
    [Required]
    [ForeignKey("UserIdFk")]
    [Column("user_id")]
    public int UserIdFk { get; set; }
    
    public Users User { get; set; }
    
    [Required]
    [ForeignKey("AchievementIdFk")]
    [Column("achievement_id")]
    public int AchievementId { get; set; }
    
    public Achievements Achievement { get; set; }
    
    [Required]
    [Column("earned_at")]
    [Timestamp, DataType("timestamp")]
    public byte[] EarnedAt { get; set; }
}