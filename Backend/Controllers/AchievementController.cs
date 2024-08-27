using Backend.Dtos;
using Backend.Services;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
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
            return Ok(new { message = "Achievements retrieved successfully", achievements = await _achievementService.Retrieve() });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving achievement", error = e.Message });
        }
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetAchievement(int id)
    {
        try
        {
            return Ok(new { message = "Achievement retrieved successfully", achievement = await _achievementService.Retrieve(id) });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving achievement", error = e.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddAchievement([FromBody] AchievementDto dto)
    {
        if (!DtoValidator.ValidateObject(dto, out var message))
        {
            return BadRequest(message);
        }
        try
        {
            await _achievementService.AddAchievement(dto);
            return BadRequest(new { message = "Achievement added successfully" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in adding achievement", error = e.Message });
        }
    }
    
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditAchievement(int id, [FromBody] EditAchievementDto dto)
    {
        if (!DtoValidator.ValidateObject(dto, out var message))
        {
            return BadRequest(message);
        }
        try
        {
            return Ok(new { message = "Achievement edited successfully", achievement = await _achievementService.EditAchievement(id, dto) });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in editing achievement", error = e.Message });
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
            return Ok(new { message = "Achievement deleted successfully" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in deleting achievement", error = e.Message });
        }
    }
}