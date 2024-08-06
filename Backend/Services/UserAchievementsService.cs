using System.Reflection;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories;

namespace Backend.Services;

public class UserAchievementsService
{
    private readonly UserAchievementsRepository _userAchievementsRepository;
    
    public UserAchievementsService(UserAchievementsRepository userAchievementsRepository)
    {
        _userAchievementsRepository = userAchievementsRepository;
    }
    
    public async Task<IEnumerable<UserAchievementsDtos>> Retrieve()
    {
        var existingUserAchievements = await _userAchievementsRepository.GetUserAchievements();
        return existingUserAchievements.Select(u => u.ConvertToDto()).ToList();
    }
    
    public async Task<UserAchievementsDtos> Retrieve(int id)
    {
        var existingUserAchievements = await _userAchievementsRepository.GetUserAchievementsById(id);
        return existingUserAchievements.ConvertToDto();
    }
    
    public async Task<UserAchievementsDtos> AddUserAchievement(UserAchievementsDtos userAchievementsDto)
    {
        var userAchievements = userAchievementsDto.ConvertToEntity();
        await _userAchievementsRepository.AddUserAchievements(userAchievements);
        
        return userAchievementsDto;
    }
    
    public async Task<UserAchievementsDtos> EditUserAchievement(int id, EditUserAchievementDtos userAchievementsDto)
    {
        var existingUserAchievements = await _userAchievementsRepository.GetUserAchievementsById(id);
        UpdatePropertiesIfNeeded(existingUserAchievements, userAchievementsDto, []);
        await _userAchievementsRepository.EditUserAchievements(id, existingUserAchievements);
        
        return existingUserAchievements.ConvertToDto();
    }
    
    public async Task<UserAchievementsDtos> DeleteUserAchievement(int id)
    {
        var existingUserAchievements = await _userAchievementsRepository.GetUserAchievementsById(id);
        await _userAchievementsRepository.DeleteUserAchievements(id);
        
        return existingUserAchievements.ConvertToDto();
    }
    
    private void UpdatePropertiesIfNeeded<T>(UserAchievements userAchievements, T userAchievementsDto, string[] excludedProperties)
    {
        var userProperties = typeof(UserAchievements).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.CanRead && prop.CanWrite && !excludedProperties.Contains(prop.Name));
        var dtoProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => !excludedProperties.Contains(prop.Name));
        
        foreach (var userProp in userProperties)
        {
            var dtoProp = dtoProperties.FirstOrDefault(p => p.Name == userProp.Name && p.PropertyType == userProp.PropertyType);
            if (dtoProp == null) continue;
            
            var sourceValue = dtoProp.GetValue(userAchievementsDto);
            var targetValue = userProp.GetValue(userAchievements);
            
            if (!Equals(sourceValue, targetValue) && sourceValue != null)
            {
                userProp.SetValue(userAchievements, sourceValue);
            }
        }
    }
}