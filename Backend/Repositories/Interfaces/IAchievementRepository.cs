using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface IAchievementRepository
{
    Task<Achievement?> GetAchievementById(int id);
    Task<Achievement?> GetAchievementByName(string name); 
    Task<IEnumerable<Achievement>> GetAllAchievements();
    Task AddAchievement(Achievement achievement);
    Task DeleteAchievementById(int id);
    Task EditAchievement(Achievement achievement);
    Task SaveChanges();
}