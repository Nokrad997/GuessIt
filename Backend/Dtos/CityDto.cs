using Backend.Dtos.EditDtos;
using Backend.Entities;

namespace Backend.Dtos;

public class CityDto : EditCityDto
{
    public int CityId { get; set; }

    public City ConvertToEntity()
    {
        return new City
        {
            CityId = CityId,
            CityName = CityName,
            CountryIdFk = CountryIdFk,
            GeolocationIdFk = GeolocationIdFk
        };
    }
}