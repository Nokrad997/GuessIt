using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class CityController : ControllerBase
{
    private readonly CityService _cityService;
    
    public CityController(CityService cityService)
    {
        _cityService = cityService;
    }
    
    [HttpGet("{cityId:int}")]
    public async Task<IActionResult> GetCityById(int cityId)
    {
        try
        {
            return Ok(new {message = "City retrieved successfully", city = await _cityService.Retrieve(cityId)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in retrieving city", error = e.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCities()
    {
        try
        {
            return Ok(new {message = "Cities retrieved successfully", cities = await _cityService.Retrieve()});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving city", error = e.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddCity([FromBody] CityDto cityDto)
    {
        if(!DtoValidator.ValidateObject(cityDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            await _cityService.AddCity(cityDto);
            return Ok(new {message = "City created successfully"});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in adding city", error = e.Message });
        }
    }
    
    [HttpPut("{cityId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditCity(int cityId, [FromBody] EditCityDto editCityDto)
    {
        if(!DtoValidator.ValidateObject(editCityDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            return Ok(new {message = "City updated successfully", city = await _cityService.EditCity(cityId, editCityDto)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in editing city", error = e.Message });
        }
    }
    
    [HttpDelete("{cityId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCity(int cityId)
    {
        try
        {
            await _cityService.DeleteCity(cityId);
            return Ok(new { message = "City deleted successfully" });
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in deleting city", error = e.Message });
        }
    }
}