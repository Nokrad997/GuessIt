using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using Backend.Services.Interfaces;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly ICountryService _countryService;
    
    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }
    
    [HttpGet("{countryId:int}")]
    public async Task<IActionResult> GetCountryById(int countryId)
    {
        try
        {
            return Ok(new {message = "Country retrieved successfully", country = await _countryService.Retrieve(countryId)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in retrieving country", error = e.Message });
        }
    }
    
    [HttpGet("by-continent/{continentId:int}")]
    public async Task<IActionResult> GetCountryByContinentId(int continentId)
    {
        try
        {
            return Ok(new {message = "Countries retrieved successfully", countries = await _countryService.RetrieveByContinentId(continentId)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in retrieving countries", error = e.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCountries()
    {
        try
        {
            return Ok(new {message = "Countries retrieved successfully", countries = await _countryService.Retrieve()});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving country", error = e.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCountry([FromBody] CountryDto countryDto)
    {
        if(!DtoValidator.ValidateObject(countryDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            await _countryService.AddCountry(countryDto);
            return Ok(new {message = "Country created successfully"});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in adding country", error = e.Message });
        }
    }
    
    [HttpPut("{countryId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCountry(int countryId, [FromBody] EditCountryDto editCountryDto)
    {
        if(!DtoValidator.ValidateObject(editCountryDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            return Ok(new {message = "Country updated successfully", country = await _countryService.EditCountry(countryId, editCountryDto)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in editing country", error = e.Message });
        }
    }
    
    [HttpDelete("{countryId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCountry(int countryId)
    {
        try
        {
            await _countryService.DeleteCountry(countryId);
            return Ok(new {message = "Country deleted successfully"});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = "Failed in deleting country", error = e.Message });
        }
    }
}