using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Repositories;

namespace Backend.Services;

public class ContinentService
{
    private readonly ContinentRepository _continentRepository;
    
    public ContinentService(ContinentRepository continentRepository)
    {
        _continentRepository = continentRepository;
    }
    
    public async Task<ContinentDto> Retrieve(int continentId)
    {
        var continent = await _continentRepository.GetContinentById(continentId);
        if (continent == null)
        {
            throw new ArgumentException("Continent with provided id not found");
        }
        
        return continent.ConvertToDto();
    }
    
    public async Task<IEnumerable<ContinentDto>> Retrieve()
    {
        var continents = await _continentRepository.GetContinents();
        return continents.Select(c => c.ConvertToDto());
    }
    
    public async Task AddContinent(ContinentDto continentDto)
    {
        var continent = await _continentRepository.GetContinentByGeolocationId(continentDto.GeolocationIdFk);
        if (continent != null)
        {
            throw new ArgumentException("Continent with provided geolocation id already exists");
        }
        
        await _continentRepository.AddContinent(continentDto.ConvertToEntity());
    }
    
    public async Task<ContinentDto> EditContinent(int continentId, EditContinentDto editContinentDto)
    {
        var continent = await _continentRepository.GetContinentById(continentId);
        if (continent == null)
        {
            throw new ArgumentException("Continent with provided id not found");
        }
        
        continent.ContinentName = editContinentDto.ContinentName;
        continent.GeolocationIdFk = editContinentDto.GeolocationIdFk;
        
        await _continentRepository.EditContinent(continent);
        
        return continent.ConvertToDto();
    }
    
    public async Task DeleteContinent(int continentId)
    {
        var continent = await _continentRepository.GetContinentById(continentId);
        if (continent == null)
        {
            throw new ArgumentException("Continent with provided id not found");
        }
        
        await _continentRepository.DeleteContinent(continent);
    }
}