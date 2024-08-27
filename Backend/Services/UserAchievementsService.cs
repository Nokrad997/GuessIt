using System.Reflection;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Exceptions;
using Backend.Repositories;

namespace Backend.Services;

public class UserAchievementsService
{
    private readonly UserAchievementsRepository _userAchievementsRepository;
    private readonly UserRepository _userRepository;
    private readonly AchievementRepository _achievementRepository;
    
    public UserAchievementsService(UserAchievementsRepository userAchievementsRepository, UserRepository userRepository, AchievementRepository achievementRepository)
    {
        _userAchievementsRepository = userAchievementsRepository;
        _userRepository = userRepository;
        _achievementRepository = achievementRepository;
    }
    
    public async Task<IEnumerable<UserAchievementsDtos>> Retrieve()
    {
        var existingUserAchievements = await _userAchievementsRepository.GetUserAchievements();
        return existingUserAchievements.Select(u => u.ConvertToDto()).ToList();
    }
    
    public async Task<UserAchievementsDtos> Retrieve(int id)
    {
        var existingUserAchievements = await _userAchievementsRepository.GetUserAchievementsById(id);
        if (existingUserAchievements is null)
        {
            throw new ArgumentException("UserAchievements not found");
        }
        return existingUserAchievements.ConvertToDto();
    }
    
    public async Task AddUserAchievement(UserAchievementsDtos userAchievementsDto)
    {
        var existingUserAchievement = await _userAchievementsRepository.GetUserAchievementsById(userAchievementsDto.UserAchievementId);
        if (existingUserAchievement is not null)
        {
            throw new EntryAlreadyExistsException();
        }
        
        var existingUser = await _userRepository.GetUserById(userAchievementsDto.UserIdFk);
        if (existingUser is null)
        {
            throw new ArgumentException("User with given id does not exist");
        }

        var existingAchievement = _achievementRepository.GetAchievementById(userAchievementsDto.AchievementIdFk);
        if (existingAchievement is null)
        {
            throw new ArgumentException("Achievement with given id does not exist");
        }
        
        await _userAchievementsRepository.AddUserAchievements(userAchievementsDto.ConvertToEntity());
    }
    
    public async Task<UserAchievementsDtos> EditUserAchievement(int id, EditUserAchievementDtos userAchievementsDto)
    {
        var existingUserAchievements = await _userAchievementsRepository.GetUserAchievementsById(id);
        if (existingUserAchievements is null)
        {
            throw new ArgumentException("UserAchievement not found");
        }
        
        var existingUser = await _userRepository.GetUserById(userAchievementsDto.UserIdFk);
        if (existingUser is null)
        {
            throw new ArgumentException("User with given id does not exist");
        }

        var existingAchievement = _achievementRepository.GetAchievementById(userAchievementsDto.AchievementIdFk);
        if (existingAchievement is null)
        {
            throw new ArgumentException("Achievement with given id does not exist");
        }
        
        UpdatePropertiesIfNeeded(existingUserAchievements, userAchievementsDto, []);
        await _userAchievementsRepository.EditUserAchievements(existingUserAchievements);
        
        return existingUserAchievements.ConvertToDto();
    }
    
    public async Task DeleteUserAchievement(int id)
    {
        var existingUserAchievements = await _userAchievementsRepository.GetUserAchievementsById(id);
        if (existingUserAchievements is null)
        {
            throw new ArgumentException("UserAchievement not found");
        }
        
        await _userAchievementsRepository.DeleteUserAchievements(existingUserAchievements);
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