using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;

namespace Backend.Services.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameDto>> Retrieve();
    Task<GameDto> Retrieve(int id);
    Task AddGame(GameDto gameDto);
    Task AddAfterGameStatistics(GameDto gameDto, string gameType, string token);
    Task<GameDto> EditGame(int id, EditGameDto editGameDto);
    Task DeleteGame(int id);
}