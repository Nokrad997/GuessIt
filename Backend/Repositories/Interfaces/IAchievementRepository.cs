using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface IAchievementRepository
{
    Task<Achievement?> GetAchievementById(int id);
    Task<Achievement?> GetAchievementByName(string name); 
    Task<IEnumerable<Achievement>> GetAllAchievements();
    Task<Achievement?> GetAchievementByCriteria(Dictionary<string, object> criteria);
    Task<IEnumerable<Achievement>> GetAchievementsByIds(IEnumerable<int> ids);
    Task AddAchievement(Achievement achievement);
    Task AddAchievementsInBulk(IEnumerable<Achievement> achievements);
    Task DeleteAchievementById(int id);
    Task EditAchievement(Achievement achievement);
    Task SaveChanges();
}