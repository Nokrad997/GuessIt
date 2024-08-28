using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface IStatisticsRepository
{
    Task<Statistics?> GetStatisticsById(int id);
    Task<IEnumerable<Statistics>> GetStatistics();
    Task<Statistics?> GetStatisticsByUserId(int userId);
    Task AddStatistics(Statistics statistics);
    Task EditStatistics(Statistics statistics);
    Task DeleteStatistics(Statistics statistics);
}