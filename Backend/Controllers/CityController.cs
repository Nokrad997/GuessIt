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
    
    [HttpGet("{cityId}")]
    public async Task<IActionResult> GetCityById(int cityId)
    {
        try
        {
            return Ok(new {message = "City retrieved successfully", city = await _cityService.Retrieve(cityId)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new {message = e.Message});
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCitys()
    {
        try
        {
            return Ok(new {message = "Citys retrieved successfully", citys = await _cityService.Retrieve()});
        }
        catch (Exception e)
        {
            return BadRequest(new {message = e.Message});
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCity([FromBody] CityDto cityDto)
    {
        if(!DtoValidator.ValidateObject(cityDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            return Ok(new {message = "City created successfully", city = await _cityService.AddCity(cityDto)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new {message = e.Message});
        }
    }
    
    [HttpPut("{cityId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCity(int cityId, [FromBody] EditCityDto editCityDto)
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
            return BadRequest(new {message = e.Message});
        }
    }
    
    [HttpDelete("{cityId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCity(int cityId)
    {
        try
        {
            return Ok(new {message = "City deleted successfully", city = await _cityService.DeleteCity(cityId)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new {message = e.Message});
        }
    }
}