using Backend.Context;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class LeaderboardRepository : ILeaderboardRepository
{
    private readonly GuessItContext _context;
    
    public LeaderboardRepository(GuessItContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Leaderboard>> GetWholeLeaderboard()
    {
        return await _context.Leaderboard.ToListAsync();
    }
    
    public async Task<Leaderboard?> GetLeaderboardById(int id)
    {
        return await _context.Leaderboard.FirstOrDefaultAsync(x => x.LeaderBoardId == id);
    }
    
    public async Task<Leaderboard?> GetLeaderboardByUserId(int userId)
    {
        return await _context.Leaderboard.FirstOrDefaultAsync(x => x.UserIdFk == userId);
    }
    
    public async Task CreateLeaderboardEntry(Leaderboard leaderboard)
    {
        var existingLeaderboard = await _context.Leaderboard.FirstOrDefaultAsync(x => x.UserIdFk == leaderboard.UserIdFk);
        if (existingLeaderboard is not null)
        {
            throw new ArgumentException("Leaderboard entry already exists for this user");   
        }
        
        await _context.Leaderboard.AddAsync(leaderboard);
        await SaveChanges();
    }
    
    public async Task EditLeaderboardEntry(Leaderboard leaderboard)
    {
        var existingLeaderboard = await _context.Leaderboard.FirstOrDefaultAsync(x => x.UserIdFk == leaderboard.UserIdFk);
        if (existingLeaderboard is null)
        {
            throw new ArgumentException("Leaderboard entry does not exist for this user");
        }
        
        existingLeaderboard.TotalPoints = leaderboard.TotalPoints;
        await SaveChanges();
    }
    
    public async Task DeleteLeaderboardEntry(int id)
    {
        var leaderboard = await _context.Leaderboard.FirstOrDefaultAsync(x => x.LeaderBoardId == id);
        if (leaderboard is null)
        {
            throw new ArgumentException("Leaderboard entry does not exist");
        }
        
        _context.Leaderboard.Remove(leaderboard);
        await SaveChanges();
    }
    
    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}