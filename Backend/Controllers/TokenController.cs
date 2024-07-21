using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
namespace Backend.Controllers;

[Route("token")]
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
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null)
        {
            return BadRequest("Token is missing");
        }

        if (_tokenService.Validate(token))
        {
            return Ok("Valid token");
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

        var newToken = _tokenService.RefreshToken(tokens.RefreshToken);

        if (!newToken.IsNullOrEmpty())
        {
            return Ok(newToken);
        }

        return BadRequest("token no longer valid");
    }
}