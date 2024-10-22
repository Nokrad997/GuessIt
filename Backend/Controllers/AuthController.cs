using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
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
    public async Task<IActionResult> RegisterUser([FromBody] EditUserDto registerUserDto)
    {
        if (!DtoValidator.ValidateObject(registerUserDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            await _authService.RegisterUser(registerUserDto);
            return Ok(new { message = "User registered successfully" });;
        }
        catch(Exception e)
        {
            return BadRequest(new { message = "Failed in registering", error = e.Message });
        }
    }

    [Route("login")]
    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> LoginUser([FromBody] AuthUserDto authUserDto)
    {
        if(!DtoValidator.ValidateObject(authUserDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            var jwtToken = await _authService.LoginUser(authUserDto);
            return Ok(new { accessToken = jwtToken.AccessToken, refreshToken = jwtToken.RefreshToken });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in logging in", error = e.Message });
        }
    }
}