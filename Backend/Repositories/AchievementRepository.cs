using Backend.Context;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AchievementRepository : IAchievementRepository
{
    private readonly GuessItContext _context;

    public AchievementRepository(GuessItContext context)
    {
        _context = context;
    }
    
    public async Task AddAchievement(Achievement achievement)
    {
        await _context.Achievement.AddAsync(achievement);
        await SaveChanges();
    }

    public async Task<Achievement> GetAchievementById(int id)
    {
        return await _context.Achievement.FirstOrDefaultAsync(achievement => achievement.AchievementId == id); 
    }
    public async Task<Achievement> GetAchievementByName(string name)
    {
        return await _context.Achievement.FirstOrDefaultAsync(achievement => achievement.AchievementName == name); 
    }
    public async Task<IEnumerable<Achievement>> GetAllAchievements()
    {
        return await _context.Achievement.ToListAsync();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task EditAchievement(Achievement achievement)
    {
        var editedAchievement = await _context.Achievement.FindAsync(achievement.AchievementId);
        if (editedAchievement is not null)
        {
            editedAchievement = achievement;
            await SaveChanges();    
        }
    }

    public async Task DeleteAchievementById(int id)
    {
        var achievement = await _context.Achievement.FindAsync(id);

        if (achievement is not null)
        {
            _context.Achievement.Remove(achievement);
            await SaveChanges();
        }
        else
        {
            throw new KeyNotFoundException("Achievement not found");
        }
    }
}