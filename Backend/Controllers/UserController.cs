using System.ComponentModel.DataAnnotations;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend.Controllers;

[Route("api/user")]
[Controller]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly DtoValidator _dtoValidator;

    public UserController(UserService userService)
    {
        _userService = userService;
        _dtoValidator = new DtoValidator();
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
            return BadRequest(new { message = e.Message });
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
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpPost]
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddUserAsAdmin([FromBody] UserDto userDto)
    {
        if(!DtoValidator.ValidateObject(userDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            await _userService.AddUserAsAdmin(userDto);
            return Ok(new {message = "User added successfully"});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> EditUserAsUser(int id, [FromBody] EditUserDto editUserDto)
    {
        if(!DtoValidator.ValidateObject(editUserDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault().Split(" ").Last();
            return Ok(await _userService.EditUserAsUser(id, editUserDto, token));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    [HttpPut]
    [Route("admin/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditUserAsAdmin(int id, [FromBody] EditUserDto editUserDto)
    {
        if(!DtoValidator.ValidateObject(editUserDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            return Ok(await _userService.EditUserAsAdmin(id, editUserDto));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
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
            return Ok(new { messsage = "User deleted successfully" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    
    
}