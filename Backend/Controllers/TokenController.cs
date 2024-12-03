using Backend.Dtos;
using Backend.Services;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [Route("validate")]
    [HttpPost]
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
            
            return BadRequest("Token is invalid");
        }
        catch (Exception e)
        {
            BadRequest(new { message = "Failed in validating token", error = e.Message });
        }
        
        return BadRequest(new { message = "Token is invalid" });
    }

    [Route("refresh")]
    [HttpPost]
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
            return BadRequest(new { message = "Failed in refreshing token", error = e.Message });
        }
    }
}