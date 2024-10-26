using Backend.Dtos.EditDtos;
using Backend.Dtos.Interfaces;
using Backend.Services;
using Backend.Services.Interfaces;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;
    
    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetWholeLeaderboard()
    {
        try
        {
            return Ok(new {message = "Successfully retrieved leaderboard", leaderboard = await _leaderboardService.Retrieve()});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving leaderboard entry", error = e.Message });
        }
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetLeaderboardById(int id)
    {
        try
        {
            return Ok(new {message = "Successfully retrieved leaderboard", leaderboard = await _leaderboardService.Retrieve(id)});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving leaderboard entry", error = e.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddLeaderboardEntry([FromBody] LeaderboardDto leaderboardDto)
    {
        if(!DtoValidator.ValidateObject(leaderboardDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            await _leaderboardService.AddLeaderboardEntry(leaderboardDto);
            return Ok(new {message = "Successfully added leaderboard entry"});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in adding new leaderboard entry", error = e.Message });
        }
    }
    
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditLeaderboardEntry(int id, [FromBody] EditLeaderboardDto editLeaderboardDto)
    {
        if(!DtoValidator.ValidateObject(editLeaderboardDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            return Ok(new {message = "Successfully edited leaderboard entry", leaderboard = await _leaderboardService.EditLeaderBoardEntry(id, editLeaderboardDto)});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in editing leaderboard entry", error = e.Message });
        }
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteLeaderboardEntry(int id)
    {
        try
        {
            await _leaderboardService.DeleteLeaderboardEntry(id);
            return Ok(new {message = "Successfully deleted leaderboard entry"});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in deleting leaderboard entry", error = e.Message });
        }
    }
}