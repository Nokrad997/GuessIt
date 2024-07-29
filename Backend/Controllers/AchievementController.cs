using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/achievement")]
public class AchievementController : ControllerBase
{
    private readonly AchievementService _achievementService;

    public AchievementController(AchievementService achievementService)
    {
        _achievementService = achievementService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAchievements()
    {
        try
        {
            return Ok(await _achievementService.Retrieve());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetAchievement(int id)
    {
        try
        {
            return Ok(await _achievementService.Retrieve(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddAchievement(AchievementDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _achievementService.AddAchievement(dto);
            return Ok("Achievement added successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditAchievement(int id, AchievementDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            return Ok(await _achievementService.EditAchievement(id, dto));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message + "\n" + e.StackTrace);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAchievement(int id)
    {
        try
        {
            await _achievementService.DeleteAchievement(id);
            return Ok("Achievement deleted successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}