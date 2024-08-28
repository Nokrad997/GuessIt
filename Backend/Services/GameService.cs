using System.Reflection;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories;

namespace Backend.Services;

public class GameService
{
    private readonly GameRepository _gameRepository;
    private readonly UserRepository _userRepository;
    
    public GameService(GameRepository gameRepository, UserRepository userRepository)
    {
        _gameRepository = gameRepository;
        _userRepository = userRepository;
    }
    
    public async Task<IEnumerable<GameDto>> Retrieve()
    {
        var existingGames = await _gameRepository.GetGames();
        return existingGames.Select(g => g.ConvertToDto()).ToList();
    }
    
    public async Task<GameDto> Retrieve(int id)
    {
        var existingGame = await _gameRepository.GetGameById(id);
        if(existingGame == null)
        {
            throw new ArgumentException("Game not found");
        }
        
        return existingGame.ConvertToDto();
    }
    
    public async Task AddGame(GameDto gameDto)
    {
        var user = await _userRepository.GetUserById(gameDto.UserIdFk);
        if(user is null)
        {
            throw new ArgumentException("User not found");
        }
        
        await _gameRepository.AddGame(gameDto.ConvertToEntity());
    }
    
    public async Task<GameDto> EditGame(int id, EditGameDto editGameDto)
    {
        var existingGame = await _gameRepository.GetGameById(id);
        if(existingGame is null)
        {
            throw new ArgumentException("Game not found");
        }
        
        var existingUser = await _userRepository.GetUserById(editGameDto.UserIdFk);
        if(existingUser is null)
        {
            throw new ArgumentException("User not found");
        }
        
        UpdatePropertiesIfNeeded(existingGame, editGameDto, ["GameId"]);
        
        var updatedGame = await _gameRepository.EditGame(existingGame);
        return updatedGame.ConvertToDto();
    }

    public async Task DeleteGame(int id)
    {
        var existingGame = await _gameRepository.GetGameById(id);
        if(existingGame == null)
        {
            throw new ArgumentException("Game not found");
        }
        
        await _gameRepository.DeleteGame(existingGame);
    }
    
    private void UpdatePropertiesIfNeeded<T>(Game game, T gamesDto, string[] excludedProperties)
    {
        var userProperties = typeof(Game).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.CanRead && prop.CanWrite && !excludedProperties.Contains(prop.Name));
        var dtoProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => !excludedProperties.Contains(prop.Name));
        
        foreach (var userProp in userProperties)
        {
            var dtoProp = dtoProperties.FirstOrDefault(p => p.Name == userProp.Name && p.PropertyType == userProp.PropertyType);
            if (dtoProp == null) continue;
            
            var sourceValue = dtoProp.GetValue(gamesDto);
            var targetValue = userProp.GetValue(game);
            
            if (!Equals(sourceValue, targetValue) && sourceValue != null)
            {
                userProp.SetValue(game, sourceValue);
            }
        }
    }
}