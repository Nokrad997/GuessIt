using Backend.Dtos.EditDtos;
using Backend.Entities;

namespace Backend.Dtos;

public class ContinentDto : EditContinentDto
{
    public int ContinentId { get; set; }
    
    public new Continent ConvertToEntity()
    {
        return new Continent
        {
            ContinentId = ContinentId,
            ContinentName = ContinentName,
            GeolocationIdFk = GeolocationIdFk
        };
    }
}