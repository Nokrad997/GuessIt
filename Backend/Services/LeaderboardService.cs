using Backend.Dtos.EditDtos;
using Backend.Dtos.Interfaces;
using Backend.Repositories;

namespace Backend.Services;

public class LeaderboardService
{
    private readonly LeaderboardRepository _leaderboardRepository;
    
    public LeaderboardService(LeaderboardRepository leaderboardRepository)
    {
        _leaderboardRepository = leaderboardRepository;
    }
    
    public async Task<IEnumerable<LeaderboardDto>> Retrieve()
    {
        var leaderboard = await _leaderboardRepository.GetWholeLeaderboard();
        return leaderboard.Select(x => x.ConvertToDto()).ToList();
    }
    
    public async Task<LeaderboardDto> Retrieve(int id)
    {
        var leaderboard = await _leaderboardRepository.GetLeaderboardById(id);
        if (leaderboard is null)
        {
            throw new ArgumentException("Leaderboard entry does not exist");
        }
        
        return leaderboard.ConvertToDto();
    }
    
    public async Task AddLeaderboardEntry(LeaderboardDto leaderboardDto)
    {
        var leaderboard = await _leaderboardRepository.GetLeaderboardByUserId(leaderboardDto.UserIdFk);
        if (leaderboard is not null)
        {
            throw new ArgumentException("Leaderboard entry already exists for this user");
        }
        
        await _leaderboardRepository.CreateLeaderboardEntry(leaderboardDto.ConvertToEntity());
    }

    public async Task<LeaderboardDto> EditLeaderBoardEntry(int id, EditLeaderboardDto editLeaderboardDto)
    {
        var existingLeaderboardEntry = await _leaderboardRepository.GetLeaderboardById(id);
        if (existingLeaderboardEntry is null)
        {
            throw new ArgumentException("Leaderboard entry does not exist");
        }
        
        existingLeaderboardEntry.TotalPoints = editLeaderboardDto.TotalPoints;
        await _leaderboardRepository.EditLeaderboardEntry(existingLeaderboardEntry);
        
        return existingLeaderboardEntry.ConvertToDto();
    }
    
    public async Task DeleteLeaderboardEntry(int id)
    {
        var existingLeaderboardEntry = await _leaderboardRepository.GetLeaderboardById(id);
        if (existingLeaderboardEntry is null)
        {
            throw new ArgumentException("Leaderboard entry does not exist");
        }
        
        await _leaderboardRepository.DeleteLeaderboardEntry(id);
    }
}