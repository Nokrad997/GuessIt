using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Repositories;

namespace Backend.Services;

public class ContinentService
{
    private readonly ContinentRepository _continentRepository;
    private readonly GeolocationRepository _geolocationRepository;
    
    public ContinentService(ContinentRepository continentRepository, GeolocationRepository geolocationRepository)
    {
        _continentRepository = continentRepository;
        _geolocationRepository = geolocationRepository;
    }
    
    public async Task<ContinentDto> Retrieve(int continentId)
    {
        var continent = await _continentRepository.GetContinentById(continentId);
        if (continent is null)
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
        if (continent is not null)
        {
            throw new ArgumentException("Continent with provided geolocation id already exists");
        }
        if(await _continentRepository.GetContinentByName(continentDto.ContinentName) is not null)
        {
            throw new ArgumentException("Continent with provided name already exists");
        }
        if(await _geolocationRepository.GetGeolocationById(continentDto.GeolocationIdFk) is null)
        {
            throw new ArgumentException("Geolocation with provided id not found");
        }
        
        await _continentRepository.AddContinent(continentDto.ConvertToEntity());
    }
    
    public async Task<ContinentDto> EditContinent(int continentId, EditContinentDto editContinentDto)
    {
        var continent = await _continentRepository.GetContinentById(continentId);
        if (continent is null)
        {
            throw new ArgumentException("Continent with provided id not found");
        }
        if(await _continentRepository.GetContinentByName(editContinentDto.ContinentName) is not null)
        {
            throw new ArgumentException("Continent with provided name already exists");
        }
        if(await _geolocationRepository.GetGeolocationById(editContinentDto.GeolocationIdFk) is null)
        {
            throw new ArgumentException("Geolocation with provided id not found");
        }
        
        continent.ContinentName = editContinentDto.ContinentName;
        continent.GeolocationIdFk = editContinentDto.GeolocationIdFk;
        
        await _continentRepository.EditContinent(continent);
        
        return continent.ConvertToDto();
    }
    
    public async Task DeleteContinent(int continentId)
    {
        var continent = await _continentRepository.GetContinentById(continentId);
        if (continent is null)
        {
            throw new ArgumentException("Continent with provided id not found");
        }
        
        await _continentRepository.DeleteContinent(continent);
    }
}