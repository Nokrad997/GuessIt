using Backend.Dtos.EditDtos;
using Backend.Dtos.Interfaces;
using Backend.Services;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly LeaderboardService _leaderboardService;
    
    public LeaderboardController(LeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetWholeLeaderboard()
    {
        try
        {
            return Ok(new {message = "Successfully retrieved leaderboard", leaderboard = await _leaderboardService.GetWholeLeaderboard()});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetLeaderboardById(int id)
    {
        try
        {
            return Ok(new {message = "Successfully retrieved leaderboard", leaderboard = await _leaderboardService.GetLeaderboardById(id)});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
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
            return Ok(new {message = "Successfully added leaderboard entry", leaderboard = await _leaderboardService.AddLeaderboardEntry(leaderboardDto)});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
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
            return BadRequest(new { message = e.Message });
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
            return BadRequest(new { message = e.Message });
        }
    }
}