using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface IAchievementRepository
{
    Task<Achievement> GetAchievementById(int id);
    Task<IEnumerable<Achievement>> GetAllAchievements();
    Task AddAchievement(Achievement user);
    Task DeleteAchievementById(int id);
    Task EditAchievement(Achievement user);
    Task SaveChanges();
}