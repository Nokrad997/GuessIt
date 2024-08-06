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
    
    public async Task<UserAchievements> GetUserAchievementsById(int id)
    {
        var existingUserAchievements = await _context.UserAchievements.FirstOrDefaultAsync(u => u.UserAchievementId == id);
        if (existingUserAchievements is null)
        {
            throw new KeyNotFoundException("UserAchievement for this id does not exist");
        }
        
        return existingUserAchievements;
    }

    public async Task<IEnumerable<UserAchievements>> GetUserAchievements()
    {
        var existingUserAchievements = await _context.UserAchievements.ToListAsync();
        return existingUserAchievements;
    }

    public async Task<UserAchievements> GetUserAchievementsByUserId(int userId)
    {
        var existingUserAchievement = await _context.UserAchievements.FirstOrDefaultAsync(u => u.UserIdFk == userId);
        if (existingUserAchievement is null)
        {
            throw new KeyNotFoundException("UserAchievement for this user id does not exist");
        }
        
        return existingUserAchievement;
    }

    public async Task AddUserAchievements(UserAchievements userAchievement)
    {
        var existingUserAchievement = await _context.UserAchievements.FirstOrDefaultAsync(u => u.UserIdFk == userAchievement.UserIdFk && u.AchievementIdFk == userAchievement.AchievementIdFk);
        if (existingUserAchievement != null)
        {
            throw new EntryAlreadyExistsException();
        }
        
        var existingUser = await _context.User.FirstOrDefaultAsync(u => u.UserId == userAchievement.UserIdFk);
        if (existingUser is null)
        {
            throw new KeyNotFoundException("User with given id does not exist");
        }
        
        var existingAchievement = await _context.Achievement.FirstOrDefaultAsync(a => a.AchievementId == userAchievement.AchievementIdFk);
        if (existingAchievement is null)
        {
            throw new KeyNotFoundException("Achievement with given id does not exist");
        }
        
        await _context.UserAchievements.AddAsync(userAchievement);
        await SaveChanges();
    }

    public async Task EditUserAchievements(int id, UserAchievements userAchievement)
    {
        var existingUserAchievement = await _context.UserAchievements.FirstOrDefaultAsync(u => u.UserAchievementId == id);
        if (existingUserAchievement is null)
        {
            throw new KeyNotFoundException("UserAchievement for this id does not exist");
        }
        
        var existingUser = await _context.User.FirstOrDefaultAsync(u => u.UserId == userAchievement.UserIdFk);
        if (existingUser is null)
        {
            throw new KeyNotFoundException("User with given id does not exist");
        }
        
        var existingAchievement = await _context.Achievement.FirstOrDefaultAsync(a => a.AchievementId == userAchievement.AchievementIdFk);
        if (existingAchievement is null)
        {
            throw new KeyNotFoundException("Achievement with given id does not exist");
        }

        existingUserAchievement = userAchievement;
        await SaveChanges();
    }

    public async Task DeleteUserAchievements(int id)
    {
        var existingUserAchievement = await _context.UserAchievements.FirstOrDefaultAsync(u => u.UserAchievementId == id);
        if (existingUserAchievement is null)
        {
            throw new KeyNotFoundException("UserAchievement for this id does not exist");
        }

        _context.UserAchievements.Remove(existingUserAchievement);
        await SaveChanges();
    }

    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}