using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;

[Table("user_achievements")]
public class UserAchievements
{
    [Key]
    [Required]
    [Column("user_achievement_id")]
    public int UserAchievementId { get; set; }
    
    [Required]
    [Column("user_id")]
    [ForeignKey("UserId")]
    public virtual Users UserId { get; set; }
    
    [Required]
    [Column("achievement_id")]
    [ForeignKey("AchievementId")]
    public virtual Achievements AchievementId { get; set; }
    
    [Required]
    [Column("earned_at")]
    [Timestamp, DataType("timestamp")]
    public byte[] EarnedAt { get; set; }
}