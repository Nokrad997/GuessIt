using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Entities.Interfaces;

namespace Backend.Entities;
[Table("leaderboard")]
public class Leaderboard : IHasTimeStamp
{
    [Key]
    [Required]
    [Column("leaderboard_id")]
    public int LeaderBoardId { get; set; }
    
    [Required]
    [ForeignKey("UserIdFk")]
    [Column("user_id")]
    public int UserIdFk { get; set; }
    
    public User User { get; set; }
    
    [Required]
    [Column("total_points")]
    public int TotalPoints { get; set; }
}