using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Entities.Interfaces;

namespace Backend.Entities;
public class Leaderboard : IHasTimeStamp
{
    public int LeaderBoardId { get; set; }
    
    public int UserIdFk { get; set; }
    public User User { get; set; }
    
    public int TotalPoints { get; set; }
}