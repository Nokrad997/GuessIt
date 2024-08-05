using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Dtos;

namespace Backend.Entities;

public class Statistics
{
    public int StatisticId { get; set; }
    
    public int UserIdFk { get; set; }
    public User User { get; set; }
    
    public int TotalGames { get; set; }
    public int TotalPoints { get; set; }
    public int HighestScore { get; set; }
    public double LowestTimeInSeconds { get; set; }
    public double TotalTraveledDistanceInMeters { get; set; }
    public double AverageScore { get; set; }

    public StatisticsDto ConvertToDto()
    {
        return new StatisticsDto
        {
            StatisticId = StatisticId,
            UserIdFk = UserIdFk,
            TotalGames = TotalGames,
            TotalPoints = TotalPoints,
            HighestScore = HighestScore,
            LowestTimeInSeconds = LowestTimeInSeconds,
            TotalTraveledDistanceInMeters = TotalTraveledDistanceInMeters,
            AverageScore = AverageScore
        };
    }
}