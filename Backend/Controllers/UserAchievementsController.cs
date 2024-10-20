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
public class UserAchievementsController : ControllerBase
{
    private readonly UserAchievementsService _userAchievementsService;
    
    public UserAchievementsController(UserAchievementsService userAchievementsService)
    {
        _userAchievementsService = userAchievementsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUserAchievements()
    {
        try
        {
            return Ok(new {message = "UserAchievements retrieved successfully", userAchievements = await _userAchievementsService.Retrieve()});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving userAchievements", error = e.Message });
        }
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetUserAchievements(int id)
    {
        try
        {
            return Ok(new {message = "UserAchievement retrieved successfully", userAchievement = await _userAchievementsService.Retrieve(id)});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving userAchievements", error = e.Message });
        }
    }

    [HttpGet]
    [Route("getUserAssociatedAchievements")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> GetUserAssociatedAchievements()
    {
        try
        {
            return Ok(new { message = "Successfully retrieved user achievements", userAchievements = await _userAchievementsService.RetrieveUserAchievements(GetTokenFromRequest(HttpContext))});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving userAchievements", error = e.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddUserAchievements([FromBody] UserAchievementsDtos userAchievementsDto)
    {
        if(!DtoValidator.ValidateObject(userAchievementsDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            await _userAchievementsService.AddUserAchievement(userAchievementsDto);
            return Ok(new { message = "UserAchievement added successfully"});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in adding userAchievements", error = e.Message });
        }
    }
    
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditUserAchievements(int id, [FromBody] EditUserAchievementDtos userAchievementsDto)
    {
        if(!DtoValidator.ValidateObject(userAchievementsDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            return Ok(new { message = "UserAchievement edited successfully", userAchievement = await _userAchievementsService.EditUserAchievement(id, userAchievementsDto) });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in editing userAchievements", error = e.Message });
        }
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUserAchievements(int id)
    {
        try
        {
            await _userAchievementsService.DeleteUserAchievement(id);
            return Ok(new { message = "UserAchievement deleted successfully" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in deleting userAchievements", error = e.Message });
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