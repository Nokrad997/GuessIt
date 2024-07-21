using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }
    
    [Route("register")]
    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> RegisterUser(AuthUserDto authUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        if (await _userService.RegisterUser(authUserDto))
        {
            return Ok("User Registered successfully");
        }
        
        return BadRequest("User with provided email or username already exists");
    }

    [Route("login")]
    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> LoginUser(AuthUserDto authUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var jwtToken = _userService.LoginUser(authUserDto).Result;
        if (jwtToken != null)
        {
            return Ok(jwtToken);
        }
        
        return BadRequest("Wrong email or password");
    }
}