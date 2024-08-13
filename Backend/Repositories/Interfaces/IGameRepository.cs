using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetGames();
    Task<Game?> GetGameById(int id);
    Task<IEnumerable<Game>> GetGamesByUser(User user);
    Task<Game> AddGame(Game game);
    Task<Game> EditGame(Game game);
    Task<Game> DeleteGame(Game game);
}