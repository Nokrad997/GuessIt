using System.Reflection;
using Backend.Dtos;
using Backend.Entities;
using Backend.Repositories;

namespace Backend.Services;

public class AchievementService
{
    private readonly AchievementRepository _achievementRepository;

    public AchievementService(AchievementRepository achievementRepository)
    {
        _achievementRepository = achievementRepository;
    }

    public async Task<IEnumerable<AchievementDto>> Retrieve()
    {
        var retrievedAchievements = await _achievementRepository.GetAllAchievements();
        
        return retrievedAchievements.Select(achievement => achievement.ConvertToDto()).ToList();
    }
    public async Task<AchievementDto> Retrieve(int id)
    {
        var retrievedAchievement = await _achievementRepository.GetAchievementById(id);
        if (retrievedAchievement is null)
        {
            throw new ArgumentException("No achievement with provided id");
        }

        return retrievedAchievement.ConvertToDto();
    }
    public async Task AddAchievement(AchievementDto dto)
    {
        var existingAchievement = await _achievementRepository.GetAchievementByName(dto.AchievementName);
        if (existingAchievement is not null)
        {
            throw new ArgumentException("Achievement with provided name already exists");
        }

        await _achievementRepository.AddAchievement(dto.ConvertToEntity());
    }
    public async Task<AchievementDto> EditAchievement(int id, AchievementDto dto)
    {
        var retrievedAchievement = await _achievementRepository.GetAchievementById(id);
        if (retrievedAchievement is null)
        {
            throw new ArgumentException("No achievement with provided id");
        }

        Console.WriteLine(dto);
        var excludedProperties = new[] { "AchievementId", "CreatedAt", "UpdatedAt" };
        UpdatePropertiesIfNeeded(retrievedAchievement, dto, excludedProperties);
        await _achievementRepository.EditAchievement(dto.ConvertToEntity());

        return retrievedAchievement.ConvertToDto();
    }
    public async Task DeleteAchievement(int id)
    {
        var achievementToDelete = await _achievementRepository.GetAchievementById(id);
        if (achievementToDelete is null)
        {
            throw new ArgumentException("Achievement doesn't exist");
        }

        await _achievementRepository.DeleteAchievementById(id);
    }
    
    private void UpdatePropertiesIfNeeded<T>(Achievement achievement, T achievementDto, string[] excludedProperties)
    {
        var userProperties = typeof(Achievement).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.CanRead && prop.CanWrite && !excludedProperties.Contains(prop.Name));
        var dtoProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => !excludedProperties.Contains(prop.Name));
        
        foreach (var userProp in userProperties)
        {
            var dtoProp = dtoProperties.FirstOrDefault(p => p.Name == userProp.Name && p.PropertyType == userProp.PropertyType);
            if (dtoProp == null) continue;
            
            var sourceValue = dtoProp.GetValue(achievementDto);
            var targetValue = userProp.GetValue(achievement);
            System.Diagnostics.Debug.WriteLine(sourceValue);
            System.Diagnostics.Debug.WriteLine(targetValue);
            if (!Equals(sourceValue, targetValue))
            {
                userProp.SetValue(achievement, sourceValue);
            }
        }
    }
}