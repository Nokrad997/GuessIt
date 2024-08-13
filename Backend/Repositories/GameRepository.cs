using Backend.Context;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class GameRepository : IGameRepository
{
    private readonly GuessItContext _context;
    
    public GameRepository(GuessItContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Game>> GetGames()
    {
        return await _context.Game.ToListAsync();
    }

    public async Task<Game?> GetGameById(int id)
    {
        return await _context.Game.FirstOrDefaultAsync(g => g.GameId == id);
    }

    public async Task<IEnumerable<Game>> GetGamesByUser(User user)
    {
        return await _context.Game.Where(g => g.UserIdFk == user.UserId).ToListAsync();
    }

    public async Task<Game> AddGame(Game game)
    {
        await _context.Game.AddAsync(game);
        await SaveChanges();
        return game;
    }

    public async Task<Game> EditGame(Game game)
    {
        _context.Game.Update(game);
        await SaveChanges();
        return game;
    }

    public async Task<Game> DeleteGame(Game game)
    {
        _context.Game.Remove(game);
        await SaveChanges();
        return game;
    }
    
    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}