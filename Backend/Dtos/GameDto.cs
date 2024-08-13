using System.ComponentModel.DataAnnotations;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using NetTopologySuite.Geometries;

namespace Backend.Dtos;

public class GameDto : EditGameDto
{
    public int GameId { get; set; }
}