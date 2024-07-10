using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities;

[Table("games")]
public class Games
{
    [Key]
    [Required]
    [Column("game_id")]
    public int GameId { get; set; }
    
    [Required]
    [ForeignKey("UserIdFk")]
    [Column("user_id")]
    public int UserIdFk { get; set; }
    
    public Users User { get; set; }
    
    [Required]
    [Column("start_latitude")]
    public decimal StartLatitude { get; set; }
    
    [Required]
    [Column("start_longitude")]
    public decimal StartLongitude { get; set; }
    
    [Required]
    [Column("guessed_latitude")]
    public decimal GuessedLatitude { get; set; }
    
    [Required]
    [Column("guessed_longitude")]
    public decimal GuessedLongitude { get; set; }
    
    [Required]
    [Column("distance_to_starting_location")]
    public decimal DistanceToStartingLocation { get; set; }

    [Required]
    [Column("start_time")]
    public TimeSpan StartTime { get; set; }

    [Required]
    [Column("end_time")]
    public TimeSpan EndTime { get; set; }
    
    [Required]
    [Column("score")]
    public int Score { get; set; }
}