using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

[Table("achievements")]
public class Achievements
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
    
    [Required]
    [Column("created_at")]
    [Timestamp, DataType("timestamp")]
    public byte[] CreatedAt { get; set; }
}