using Backend.Context;
using Backend.Entities;
using Backend.Exceptions;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly GuessItContext _context;
    
    public StatisticsRepository(GuessItContext context)
    {
        _context = context;
    }

    public async Task<Statistics?> GetStatisticsById(int id)
    {
        return await _context.Statistics.FirstOrDefaultAsync(s => s.StatisticId == id);
    }

    public async Task<IEnumerable<Statistics>> GetStatistics()
    {
        return await _context.Statistics.ToListAsync();
    }

    public async Task<Statistics?> GetStatisticsByUserId(int userId)
    {
        return await _context.Statistics.FirstOrDefaultAsync(s => s.UserIdFk == userId);
    }

    public async Task AddStatistics(Statistics statistics)
    {
        await _context.Statistics.AddAsync(statistics);
        await SaveChanges();
    }

    public async Task EditStatistics(Statistics statistics)
    {
        _context.Statistics.Update(statistics);
        await SaveChanges();
    }

    public async Task DeleteStatistics(Statistics statistics)
    {
        _context.Statistics.Remove(statistics);
        await SaveChanges();
    }
    
    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}