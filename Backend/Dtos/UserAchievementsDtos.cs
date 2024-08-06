using System.ComponentModel.DataAnnotations;
using Backend.Dtos.EditDtos;
using Backend.Entities;

namespace Backend.Dtos;

public class UserAchievementsDtos : EditUserAchievementDtos
{
    public int UserAchievementId { get; set; }
    
    public new UserAchievements ConvertToEntity()
    {
        return new UserAchievements
        {
            UserAchievementId = UserAchievementId,
            UserIdFk = UserIdFk,
            AchievementIdFk = AchievementIdFk
        };
    }
}