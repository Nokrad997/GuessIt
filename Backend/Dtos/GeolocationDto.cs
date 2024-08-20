using Backend.Dtos.EditDtos;
using Backend.Entities;
using NetTopologySuite.Geometries;

namespace Backend.Dtos;

public class GeolocationDto : EditGeolocationDto
{
    public int GeolocationId { get; set; }

    public Geolocation ConvertToEntity()
    {
        return new Geolocation
        {
            GeolocationId = GeolocationId,
            Area = Area
        };
    }
}