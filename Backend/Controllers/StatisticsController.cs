using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly StatisticsService _statisticsService;
    
    public StatisticsController(StatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetStatistics()
    {
        try
        {
            return Ok(new {message = "Successfully retrieved statistics", statistics = await _statisticsService.Retrieve()});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving statistics", error = e.Message });
        }
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetAllStatistics(int id)
    {
        try
        {
            return Ok(new {message = "Successfully retrieved statistics", statistics = await _statisticsService.Retrieve(id)});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving statistics", error = e.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> AddStatistics([FromBody] StatisticsDto statisticsDto)
    {
        if(!DtoValidator.ValidateObject(statisticsDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            await _statisticsService.AddStatistics(statisticsDto, GetTokenFromRequest(HttpContext));
            return Ok(new {message = "Successfully added statistics"});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in adding statistics", error = e.Message });
        }
    }
    
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> EditStatistics(int id, [FromBody] EditStatisticsDto statisticsDto)
    {
        if(!DtoValidator.ValidateObject(statisticsDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            return Ok(new {message = "Successfully edited statistics", statistics = await _statisticsService.EditStatistics(id, statisticsDto, GetTokenFromRequest(HttpContext))});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in editing statistics", error = e.Message });
        }
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStatistics(int id)
    {
        try
        {
            await _statisticsService.DeleteStatistics(id);
            return Ok(new {message = "Successfully deleted statistics"});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in deleting statistics", error = e.Message });
        }
    }
    
    private static string GetTokenFromRequest(HttpContext context)
    {
        var retrievedToken = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        if(IsNullOrEmpty(retrievedToken))
        {
            throw new Exception("Token not found");
        }

        return retrievedToken;
    }
}