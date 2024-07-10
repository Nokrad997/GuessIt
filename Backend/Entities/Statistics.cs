using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;

[Table("statistics")]
public class Statistics
{
    [Key]
    [Required]
    [Column("statistic_id")]
    public int StatisticId { get; set; }
    
    [Required]
    [Column("user_id")]
    [ForeignKey("UserId")]
    public virtual Users User { get; set; }
    
    [Required]
    [Column("total_games")]
    public int TotalGames { get; set; }
    
    [Required]
    [Column("total_points")]
    public int TotalPoints { get; set; }
    
    [Required]
    [Column("highest_score")]
    public int HighestScore { get; set; }
    
    [Required]
    [Column("lowest_time_in_seconds")]
    public double LowestTimeInSeconds { get; set; }
    
    [Required]
    [Column("total_traveled_distance_in_meters")]
    public double TotalTraveledDistanceInMeters { get; set; }
    
    [Required]
    [Column("average_score")]
    public double AverageScore { get; set; }
    
    
    
}