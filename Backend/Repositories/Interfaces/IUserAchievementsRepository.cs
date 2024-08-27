using Backend.Context;
using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface IUserAchievementsRepository
{
    Task<UserAchievements?> GetUserAchievementsById(int id);
    Task<IEnumerable<UserAchievements>> GetUserAchievements();
    Task<UserAchievements?> GetUserAchievementsByUserId(int userId);
    Task AddUserAchievements(UserAchievements userAchievements);
    Task EditUserAchievements(UserAchievements userAchievements);
    Task DeleteUserAchievements(UserAchievements userAchievements);
}