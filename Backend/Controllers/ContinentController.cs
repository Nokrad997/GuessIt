using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class ContinentController : ControllerBase
{
    private readonly ContinentService _continentService;
    
    public ContinentController(ContinentService continentService)
    {
        _continentService = continentService;
    }
    
    [HttpGet("{continentId:int}")]
    public async Task<IActionResult> GetContinentById(int continentId)
    {
        try
        {
            return Ok(new {message = "Continent retrieved successfully", continent = await _continentService.Retrieve(continentId)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in retrieving continent", error = e.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContinents()
    {
        try
        {
            return Ok(new {message = "Continents retrieved successfully", continents = await _continentService.Retrieve()});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving continent", error = e.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddContinent([FromBody] ContinentDto continentDto)
    {
        if(!DtoValidator.ValidateObject(continentDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            await _continentService.AddContinent(continentDto);
            return Ok(new {message = "Continent created successfully"});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in adding continent", error = e.Message });
        }
    }
    
    [HttpPut("{continentId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditContinent(int continentId, [FromBody] EditContinentDto editContinentDto)
    {
        if(!DtoValidator.ValidateObject(editContinentDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            return Ok(new {message = "Continent updated successfully", continent = await _continentService.EditContinent(continentId, editContinentDto)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in editing continent", error = e.Message });
        }
    }
    
    [HttpDelete("{continentId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteContinent(int continentId)
    {
        try
        {
            await _continentService.DeleteContinent(continentId);
            return Ok(new {message = "Continent deleted successfully"});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in deleting continent", error = e.Message });
        }
    }
}