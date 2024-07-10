using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;

[Table("games")]
public class Games
{
    [Key]
    [Required]
    [Column("game_id")]
    public int GameId { get; set; }
    
    [Required]
    [ForeignKey("UserId")]
    [Column("user_id")]
    public virtual Users UserId { get; set; }
    
    [Required]
    [ForeignKey("start_latitude")]
    public decimal StartLatitude { get; set; }
    
    [Required]
    [ForeignKey("start_longitude")]
    public decimal StartLongitude { get; set; }
    
    [Required]
    [ForeignKey("guessed_latitude")]
    public decimal GuessedLatitude { get; set; }
    
    [Required]
    [ForeignKey("guessed_longitude")]
    public decimal GuessedLongitude { get; set; }
    
    [Required]
    [ForeignKey("distance_to_starting_location")]
    public decimal DistanceToStartingLocation { get; set; }

    [Required]
    [ForeignKey("start_time")]
    public TimeSpan StartTime { get; set; }

    [Required]
    [ForeignKey("end_time")]
    public TimeSpan EndTime { get; set; }
    
    [Required]
    [ForeignKey("score")]
    public int Score { get; set; }
}