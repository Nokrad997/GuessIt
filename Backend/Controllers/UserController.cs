using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            return Ok(await _userService.Retrieve());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            return Ok(await _userService.Retrieve(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddUserAsAdmin(UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _userService.AddUserAsAdmin(userDto);
            return Ok("User added successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> EditUserAsUser(int id, EditUserDto editUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault().Split(" ").Last();
            return Ok(await _userService.EditUserAsUser(id, editUserDto, token));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpPut]
    [Route("admin/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditUserAsAdmin(int id, UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            return Ok(await _userService.EditUserAsAdmin(id, userDto));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message + "\n" + e.StackTrace);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault().Split(" ").Last();
            await _userService.DeleteUser(id, token);
            return Ok("User deleted successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}