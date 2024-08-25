using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly CountryService _countryService;
    
    public CountryController(CountryService countryService)
    {
        _countryService = countryService;
    }
    
    [HttpGet("{countryId}")]
    public async Task<IActionResult> GetCountryById(int countryId)
    {
        try
        {
            return Ok(new {message = "Country retrieved successfully", country = await _countryService.Retrieve(countryId)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new {message = e.Message});
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCountries()
    {
        try
        {
            return Ok(new {message = "Countries retrieved successfully", countrys = await _countryService.Retrieve()});
        }
        catch (Exception e)
        {
            return BadRequest(new {message = e.Message});
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
            return Ok(new {message = "Country created successfully", country = await _countryService.AddCountry(countryDto)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new {message = e.Message});
        }
    }
    
    [HttpPut("{countryId}")]
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
            return BadRequest(new {message = e.Message});
        }
    }
    
    [HttpDelete("{countryId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCountry(int countryId)
    {
        try
        {
            return Ok(new {message = "Country deleted successfully", country = await _countryService.DeleteCountry(countryId)});
        }
        catch (ArgumentException e)
        {
            return BadRequest(new {message = e.Message});
        }
    }
}