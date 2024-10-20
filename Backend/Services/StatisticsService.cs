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
    private readonly UserRepository _userRepository;
    private readonly TokenUtil _tokenUtil;
    
    public StatisticsService(StatisticsRepository statisticsRepository, UserRepository userRepository ,TokenUtil tokenUtil)
    {
        _statisticsRepository = statisticsRepository;
        _userRepository = userRepository;
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
        if (existingStatistics is null)
        {
            throw new ArgumentException("Statistics not found");
        }
        
        return existingStatistics.ConvertToDto();
    }

    public async Task<StatisticsDto> GetUserStats(string token)
    {
        var userId = _tokenUtil.GetIdFromToken(token);
        var existingUser = await _userRepository.GetUserById(userId);
        if (existingUser is null)
        {
            throw new ArgumentException("User not found");
        }

        var userStats = await _statisticsRepository.GetStatisticsByUserId(existingUser.UserId);
        if (userStats is null)
        {
            return GetEmptyStatistics();
        }

        return userStats.ConvertToDto();
    }
    
    public async Task AddStatistics(StatisticsDto statisticsDto, string token)
    { 
        if(_tokenUtil.GetIdFromToken(token) != statisticsDto.UserIdFk)
        {
            if(_tokenUtil.GetRoleFromToken(token) != "Admin")
                throw new ArgumentException("User ID in token does not match user ID in statistics");
        }
        if(await _userRepository.GetUserById(statisticsDto.UserIdFk) is null)
        {
            throw new ArgumentException("User not found");
        }
        if(await _statisticsRepository.GetStatisticsByUserId(statisticsDto.UserIdFk) is not null)
        {
            throw new ArgumentException("Statistics for user already exists");
        }
        
        await _statisticsRepository.AddStatistics(statisticsDto.ConvertToEntity());
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
        if (existingStatistics is null)
        {
            throw new ArgumentException("Statistics not found");
        }
        if(await _userRepository.GetUserById(statisticsDto.UserIdFk) is null)
        {
            throw new ArgumentException("User not found");
        }
        if(await _statisticsRepository.GetStatisticsByUserId(statisticsDto.UserIdFk) is not null)
        {
            throw new ArgumentException("Statistics for user already exists");
        }
        UpdatePropertiesIfNeeded(existingStatistics, statisticsDto, ["StatisticId", "UserIdFk"]);
        
        await _statisticsRepository.EditStatistics(existingStatistics);

        return existingStatistics.ConvertToDto();
    }
    
    public async Task DeleteStatistics(int id)
    {
        var existingStatistics = await _statisticsRepository.GetStatisticsById(id);
        if (existingStatistics is null)
        {
            throw new ArgumentException("Statistics not found");
        }
        
        await _statisticsRepository.DeleteStatistics(existingStatistics);
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

    private StatisticsDto GetEmptyStatistics()
    {
        return new StatisticsDto
        {
            AverageScore = 0,
            HighestScore = 0,
            LowestTimeInSeconds = 0,
            TotalGames = 0,
            TotalPoints = 0,
            TotalTraveledDistanceInMeters = 0
        };
    }
}