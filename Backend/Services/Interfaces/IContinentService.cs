using Backend.Dtos;
using Backend.Dtos.EditDtos;

namespace Backend.Services.Interfaces;

public interface IContinentService
{
    Task<ContinentDto> Retrieve(int continentId);
    Task<IEnumerable<ContinentDto>> Retrieve();
    Task AddContinent(ContinentDto continentDto);
    Task<ContinentDto> EditContinent(int continentId, EditContinentDto editContinentDto);
    Task DeleteContinent(int continentId);
}