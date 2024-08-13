using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Dtos;
using NetTopologySuite.Geometries;

namespace Backend.Entities;

public class Game
{
    public int GameId { get; set; }
    
    public int UserIdFk { get; set; }
    public User User { get; set; }

    public Point StartLocation { get; set; }
    public Point GuessedLocation { get; set; }
    public double DistanceToStartingLocation { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Score { get; set; }

    public GameDto ConvertToDto()
    {   
        return new GameDto
        {
            GameId = GameId,
            UserIdFk = UserIdFk,
            StartLocation = StartLocation,
            GuessedLocation = GuessedLocation,
            DistanceToStartingLocation = DistanceToStartingLocation,
            StartTime = StartTime,
            EndTime = EndTime,
            Score = Score
        };
    }
}