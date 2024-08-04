using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly TokenService _tokenService;

    public TokenController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [Route("validate")]
    [HttpPost]
    [Consumes("application/json")]
    public IActionResult ValidateToken()
    {
        var token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        if (token == null)
        {
            return BadRequest(new { message = "Token is empty" });
        }

        try
        {
            if (_tokenService.Validate(token))
            {
                return Ok(new { message = "Token is valid" });
            }
        }
        catch (Exception e)
        {
            BadRequest(new { message = e.Message });
        }
        
        return BadRequest(new { message = "Token is invalid" });
    }

    [Route("refresh")]
    [HttpPost]
    [Consumes("application/json")]
    [Authorize]
    public IActionResult RefreshAccessToken([FromBody] TokensDto tokens)
    {
        if (tokens.RefreshToken.IsNullOrEmpty())
        {
            return BadRequest(new { message = "Refresh token is empty" });
        }

        try
        {
            return Ok(_tokenService.RefreshToken(tokens.RefreshToken));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}