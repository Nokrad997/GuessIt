using Backend.Dtos;
using Backend.Dtos.EditDtos;

namespace Backend.Services.Interfaces;

public interface ICityService
{
    Task<CityDto> Retrieve(int cityId);
    Task<IEnumerable<CityDto>> RetrieveByCountryId(int countryId);
    Task<IEnumerable<CityDto>> Retrieve();
    Task AddCity(CityDto cityDto);
    Task<CityDto> EditCity(int cityId, EditCityDto editCityDto);
    Task DeleteCity(int cityId);
}