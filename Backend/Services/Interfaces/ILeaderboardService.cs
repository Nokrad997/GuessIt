using Backend.Dtos.EditDtos;
using Backend.Dtos.Interfaces;

namespace Backend.Services.Interfaces;

public interface ILeaderboardService
{
    Task<IEnumerable<LeaderboardDto>> Retrieve();
    Task<LeaderboardDto> Retrieve(int id);
    Task AddLeaderboardEntry(LeaderboardDto leaderboardDto);
    Task<LeaderboardDto> EditLeaderBoardEntry(int id, EditLeaderboardDto editLeaderboardDto);
    Task DeleteLeaderboardEntry(int id);
}