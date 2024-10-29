using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using Backend.Services.Interfaces;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.String;
namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class FriendsController : ControllerBase
{
    private readonly IFriendsService _friendsService;

    public FriendsController(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllFriends()
    {
        try
        {
            return Ok(new { message = "Friends retrieved successfully", friends = await _friendsService.Retrieve() });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving friends", error = e.Message });
        }
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetFriends(int id)
    {
        try
        {
            return Ok(new { message = "Friends retrieved successfully", friends = await _friendsService.Retrieve(id) });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving friends", error = e.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> AddFriends([FromBody] FriendsDto friendDto)
    {
        if(!DtoValidator.ValidateObject(friendDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            var token = GetTokenFromRequest(HttpContext);
            await _friendsService.AddFriends(friendDto, token); 
            return Ok(new { message = "Friends added successfully"});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in adding friends", error = e.Message });
        }
    }
    
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> EditFriendsAsUser(int id, [FromBody] EditFriendsDto friendDto)
    {
        if(!DtoValidator.ValidateObject(friendDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            var token = GetTokenFromRequest(HttpContext);
            return Ok(new { message = "Friends edited successfully", friends = await _friendsService.EditFriendsAsUser(id, friendDto, token) });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in editing friends", error = e.Message });
        }
    }
    
    [HttpPut]
    [Route("admin/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditFriendsAsAdmin(int id, [FromBody] FriendsDto friendDto)
    {
        if(!DtoValidator.ValidateObject(friendDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            return Ok(new { message = "Friends edited successfully", friends = await _friendsService.EditFriendsAsAdmin(id, friendDto) });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in editing friends", error = e.Message });
        }
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFriends(int id)
    {
        try
        {
            await _friendsService.DeleteFriends(id);
            return Ok(new { message = "Friends deleted successfully" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in deleting friends", error = e.Message });
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