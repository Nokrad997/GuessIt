using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
namespace Backend.Controllers;

[Route("api/token")]
[ApiController]
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
            return BadRequest("Token is missing");
        }

        try
        {
            if (_tokenService.Validate(token))
            {
                return Ok("Token is valid");
            }
        }
        catch (Exception e)
        {
            BadRequest(e.Message);
        }
        
        return BadRequest("Invalid token");
    }

    [Route("refresh")]
    [HttpPost]
    [Consumes("application/json")]
    [Authorize]
    public IActionResult RefreshAccessToken(TokensDto tokens)
    {
        if (tokens.RefreshToken.IsNullOrEmpty())
        {
            return BadRequest("refresh token is empty");
        }

        try
        {
            return Ok(_tokenService.RefreshToken(tokens.RefreshToken));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}