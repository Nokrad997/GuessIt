using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Services;
using Backend.Services.Interfaces;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class GeolocationController : ControllerBase
{
    private readonly IGeolocationService _geolocationService;
    
    public GeolocationController(IGeolocationService geolocationService)
    {
        _geolocationService = geolocationService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGeolocationById(int id)
    {
        try
        {
            return Ok(new {message = "Geolocation retrieved successfully", geolocation = await _geolocationService.Retrieve(id)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new {message = "Failed to retrieve geolocation", error = e.Message});
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllGeolocations()
    {
        try
        {
            return Ok(new {message = "Geolocations retrieved successfully", geolocations = await _geolocationService.Retrieve()});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new {message = "Failed to retrieve geolocation", error = e.Message});
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateGeolocation([FromBody] GeolocationDto geolocationDto)
    {
        if(!DtoValidator.ValidateObject(geolocationDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            await _geolocationService.AddGeolocation(geolocationDto);
            return Ok(new {message = "Geolocation created successfully"});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new {message = "Failed to create geolocation", error = e.Message});
        }
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditGeolocation(int id, [FromBody] EditGeolocationDto editGeolocationDto)
    {
        if(!DtoValidator.ValidateObject(editGeolocationDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            return Ok(new {message = "Geolocation edited successfully", geolocation = await _geolocationService.EditGeolocation(id, editGeolocationDto)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new {message = "Failed to edit geolocation", error = e.Message});
        }
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGeolocation(int id)
    {
        try
        {
            await _geolocationService.DeleteGeolocation(id);
            return Ok(new {message = "Geolocation deleted successfully"});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new {message = "Failed to delete geolocation", error = e.Message});
        }
    }
}