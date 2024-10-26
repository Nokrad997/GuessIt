using Backend.Dtos;
using Backend.Dtos.EditDtos;

namespace Backend.Services.Interfaces;

public interface IUserAchievementService
{
    Task<IEnumerable<UserAchievementsDtos>> Retrieve();
    Task<UserAchievementsDtos> Retrieve(int id);
    Task<IEnumerable<AchievementDto>> RetrieveUserAchievements(string token);
    Task AddUserAchievement(UserAchievementsDtos userAchievementsDto);
    Task<UserAchievementsDtos> EditUserAchievement(int id, EditUserAchievementDtos userAchievementsDto);
    Task DeleteUserAchievement(int id);
    
}