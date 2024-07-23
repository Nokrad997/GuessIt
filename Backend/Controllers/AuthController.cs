using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Mvc; 

namespace Backend.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    
    [Route("register")]
    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> RegisterUser(EditUserDto registerUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            if (await _authService.RegisterUser(registerUserDto))
            {
                return Ok("User Registered successfully");
            }
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
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

        try
        {
            var jwtToken = await _authService.LoginUser(authUserDto);
            return Ok(jwtToken);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}