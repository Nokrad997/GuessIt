using Backend.Dtos;
using Backend.Dtos.EditDtos;

namespace Backend.Services.Interfaces;

public interface IStatisticsService
{
    Task<IEnumerable<StatisticsDto>> Retrieve();
    Task<StatisticsDto> Retrieve(int id);
    Task<StatisticsDto> GetUserStats(string token);
    Task AddStatistics(StatisticsDto statisticsDto, string token);
    Task<StatisticsDto> EditStatistics(int id, EditStatisticsDto statisticsDto, string token);
    Task DeleteStatistics(int id);
}