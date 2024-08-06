using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Dtos;
using Backend.Entities.Interfaces;

namespace Backend.Entities;

public class UserAchievements : IHasTimeStamp
{
    public int UserAchievementId { get; set; }
  
    public int UserIdFk { get; set; }
    public User User { get; set; }
    
    public int AchievementIdFk { get; set; }
    public Achievement Achievement { get; set; }

    public UserAchievementsDtos ConvertToDto()
    {
        return new UserAchievementsDtos
        {
            UserAchievementId = UserAchievementId,
            UserIdFk = UserIdFk,
            AchievementIdFk = AchievementIdFk
        };
    }
}