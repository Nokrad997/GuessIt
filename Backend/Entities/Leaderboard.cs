using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;
[Table("leaderboard")]
public class Leaderboard
{
    [Key]
    [Required]
    [Column("leaderboard_id")]
    public int LeaderBoardId { get; set; }
    
    [Required]
    [ForeignKey("UserIdFk")]
    [Column("user_id")]
    public int UserIdFk { get; set; }
    
    public Users User { get; set; }
    
    [Required]
    [Column("total_points")]
    public int TotalPoints { get; set; }
    
    [Required]
    [Column("last_updated")]
    [Timestamp, DataType("timestamp")]
    public byte[] LastUpdate { get; set; }
}