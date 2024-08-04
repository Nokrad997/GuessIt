using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface ILeaderboardRepository
{
    Task<IEnumerable<Leaderboard>> GetWholeLeaderboard();
    Task<Leaderboard?> GetLeaderboardById(int id);
    Task<Leaderboard?> GetLeaderboardByUserId(int userId);
    Task CreateLeaderboardEntry(Leaderboard leaderboard);
    Task EditLeaderboardEntry(Leaderboard leaderboard);
    Task DeleteLeaderboardEntry(int id);
}