using System.Reflection;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services;

public class AchievementService : IAchievementService
{
    private readonly IAchievementRepository _achievementRepository;
    
    public AchievementService(IAchievementRepository achievementRepository)
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

        existingAchievement = await _achievementRepository.GetAchievementByCriteria(dto.AchievementCriteria);
        if(existingAchievement is not null){
            throw new ArgumentException($"Achievement with provided criteria already exists: {existingAchievement.AchievementCriteria}");
        }

        await _achievementRepository.AddAchievement(dto.ConvertToEntity());
    }

    public async Task<AchievementDto> EditAchievement(int id, EditAchievementDto dto)
    {
        var retrievedAchievement = await _achievementRepository.GetAchievementById(id);
        if (retrievedAchievement is null)
        {
            throw new ArgumentException("No achievement with provided id");
        }
        if(await _achievementRepository.GetAchievementByName(dto.AchievementName) is not null)
        {
            throw new ArgumentException("Achievement with provided name already exists");
        }
        
        var excludedProperties = new[] { "AchievementId", "CreatedAt", "UpdatedAt" };
        UpdatePropertiesIfNeeded(retrievedAchievement, dto, excludedProperties);
        await _achievementRepository.EditAchievement(retrievedAchievement);

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

            if (!Equals(sourceValue, targetValue) && sourceValue != null)
            {
                userProp.SetValue(achievement, sourceValue);
            }
        }
    }
}