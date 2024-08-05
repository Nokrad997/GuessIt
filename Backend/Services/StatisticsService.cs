using System.Reflection;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories;
using Backend.Utility;

namespace Backend.Services;

public class StatisticsService
{
    private readonly StatisticsRepository _statisticsRepository;
    private readonly TokenUtil _tokenUtil;
    
    public StatisticsService(StatisticsRepository statisticsRepository, TokenUtil tokenUtil)
    {
        _statisticsRepository = statisticsRepository;
        _tokenUtil = tokenUtil;
    }

    public async Task<IEnumerable<StatisticsDto>> Retrieve()
    {
        var existingStatistics = await _statisticsRepository.GetStatistics();
        return existingStatistics.Select(s => s.ConvertToDto()).ToList();
    }

    public async Task<StatisticsDto> Retrieve(int id)
    {
        var existingStatistics = await _statisticsRepository.GetStatisticsById(id);

        return existingStatistics.ConvertToDto();
    }
    
    public async Task<StatisticsDto> AddStatistics(StatisticsDto statisticsDto, string token)
    { 
        if(_tokenUtil.GetIdFromToken(token) != statisticsDto.UserIdFk)
        {
            if(_tokenUtil.GetRoleFromToken(token) != "Admin")
                throw new ArgumentException("User ID in token does not match user ID in statistics");
        }
        
        var statistics = statisticsDto.ConvertToEntity();
        await _statisticsRepository.AddStatistics(statistics);

        return statisticsDto;
    }
    
    public async Task<StatisticsDto> EditStatistics(int id, EditStatisticsDto statisticsDto, string token)
    {
        var requestUserId = _tokenUtil.GetIdFromToken(token);
        if(requestUserId != id)
        {
            if(_tokenUtil.GetRoleFromToken(token) != "Admin")
                throw new ArgumentException("User ID in token does not match user ID in statistics");
        }
        
        var existingStatistics = await _statisticsRepository.GetStatisticsById(id);
        UpdatePropertiesIfNeeded(existingStatistics, statisticsDto, new[] { "StatisticId", "UserIdFk" });
        
        await _statisticsRepository.EditStatistics(existingStatistics);

        return existingStatistics.ConvertToDto();
    }
    
    public async Task DeleteStatistics(int id)
    {
        await _statisticsRepository.DeleteStatistics(id);
    }
    
    private void UpdatePropertiesIfNeeded<T>(Statistics statistic, T statisticsDto, string[] excludedProperties)
    {
        var userProperties = typeof(Statistics).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.CanRead && prop.CanWrite && !excludedProperties.Contains(prop.Name));
        var dtoProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => !excludedProperties.Contains(prop.Name));
        
        foreach (var userProp in userProperties)
        {
            var dtoProp = dtoProperties.FirstOrDefault(p => p.Name == userProp.Name && p.PropertyType == userProp.PropertyType);
            if (dtoProp == null) continue;
            
            var sourceValue = dtoProp.GetValue(statisticsDto);
            var targetValue = userProp.GetValue(statistic);
            
            if (!Equals(sourceValue, targetValue) && sourceValue != null)
            {
                userProp.SetValue(statistic, sourceValue);
            }
        }
    }
}