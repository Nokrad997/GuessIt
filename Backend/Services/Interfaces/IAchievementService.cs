using Backend.Dtos;

namespace Backend.Services.Interfaces;

public interface IAchievementService
{
    Task<IEnumerable<AchievementDto>> Retrieve();
    Task<AchievementDto> Retrieve(int id);
    Task AddAchievement(AchievementDto dto);
    Task<AchievementDto> EditAchievement(int id, EditAchievementDto dto);
    Task DeleteAchievement(int id);
}