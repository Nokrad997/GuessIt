using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;
[Table("leaderboard")]
public class Leaderboard
{
    [Key]
    [Required]
    [Column("leaderboard_id")]
    public int LeaderBoardId { get; set; }
    
    [Required]
    [ForeignKey("UserId")]
    [Column("leaderboard_id")]
    public virtual Users UserId { get; set; }
    
    [Required]
    [Column("total_points")]
    public int TotalPoints { get; set; }
    
    [Required]
    [Column("last_updated")]
    [Timestamp, DataType("timestamp")]
    public byte[] LastUpdate { get; set; }
}