using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Backend.Entities;

public class Game
{
    public int GameId { get; set; }
    
    public int UserIdFk { get; set; }
    public User User { get; set; }

    public Point StartLocation { get; set; }
    public Point GuessedLocation { get; set; }
    public decimal DistanceToStartingLocation { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int Score { get; set; }
}