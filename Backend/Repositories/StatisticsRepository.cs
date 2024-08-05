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

    public async Task<Statistics> GetStatisticsById(int id)
    {
        var existingStatistics = await _context.Statistics.FirstOrDefaultAsync(s => s.StatisticId == id);
        if (existingStatistics is null)
        {
            throw new KeyNotFoundException("statistics does not exist");
        }

        return existingStatistics;
    }

    public async Task<IEnumerable<Statistics>> GetStatistics()
    {
        return await _context.Statistics.ToListAsync();
    }

    public async Task<Statistics> GetStatisticsByUserId(int userId)
    {
        var existingStatistics = await _context.Statistics.FirstOrDefaultAsync(s => s.UserIdFk == userId);
        if (existingStatistics is null)
        {
            throw new KeyNotFoundException("Statistics does not exist for this user");
        }

        return existingStatistics;
    }

    public async Task AddStatistics(Statistics statistics)
    {
        var existingStatistics = await _context.Statistics.FirstOrDefaultAsync(s => s.UserIdFk == statistics.UserIdFk);
        if (existingStatistics is not null)
        {
            throw new EntryAlreadyExistsException();
        }
        
        await _context.Statistics.AddAsync(statistics);
        await SaveChanges();
    }

    public async Task EditStatistics(Statistics statistics)
    {
        var existingStatistics = await _context.Statistics.FirstOrDefaultAsync(s => s.UserIdFk == statistics.UserIdFk);
        if (existingStatistics is null)
        {
            throw new KeyNotFoundException("Statistics does not exist for this user");
        }
        
        _context.Statistics.Update(statistics);
        await SaveChanges();
    }

    public async Task DeleteStatistics(int id)
    {
        var statistics = await GetStatisticsById(id);
        if (statistics is null)
        {
            throw new KeyNotFoundException("Statistics does not exist");
        }
        
        _context.Statistics.Remove(statistics);
        await SaveChanges();
    }
    
    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}