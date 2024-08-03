using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using Backend.Utility;
using Microsoft.AspNetCore.Mvc; 

namespace Backend.Controllers;

[Route("api/auth")]
[Controller]
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
            if (await _authService.RegisterUser(registerUserDto))
            {
                return Ok(new { message = "User registered successfully" });;
            }
        }
        catch(Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
        
        return BadRequest(new { message = "User already exists" });
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
            return Ok(jwtToken);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}