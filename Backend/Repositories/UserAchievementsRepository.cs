using Backend.Context;
using Backend.Entities;
using Backend.Exceptions;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserAchievementsRepository : IUserAchievementsRepository
{
    private readonly GuessItContext _context; 
    
    public UserAchievementsRepository(GuessItContext context)
    {
        _context = context;
    }
    
    public async Task<UserAchievements?> GetUserAchievementsById(int id)
    {
        return await _context.UserAchievements.FirstOrDefaultAsync(u => u.UserAchievementId == id);
    }

    public async Task<IEnumerable<UserAchievements>> GetUserAchievements()
    {
        var existingUserAchievements = await _context.UserAchievements.ToListAsync();
        return existingUserAchievements;
    }

    public async Task<IEnumerable<UserAchievements>> GetUserAchievementsByUserId(User user)
    {
        return await _context.UserAchievements.Where(u => u.UserIdFk == user.UserId).ToListAsync();
    }

    public async Task AddUserAchievements(UserAchievements userAchievement)
    {
        await _context.UserAchievements.AddAsync(userAchievement);
        await SaveChanges();
    }

    public async Task EditUserAchievements(UserAchievements userAchievement)
    {
        _context.UserAchievements.Update(userAchievement);
        await SaveChanges();
    }

    public async Task DeleteUserAchievements(UserAchievements userAchievement)
    {
        _context.UserAchievements.Remove(userAchievement);
        await SaveChanges();
    }

    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}