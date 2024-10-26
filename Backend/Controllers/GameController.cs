using static System.String;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using Backend.Services.Interfaces;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    
    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllGames()
    {
        try
        {
            return Ok(new {message = "Games retrieved successfully", games = await _gameService.Retrieve()});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving games", error = e.Message });
        }
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetGame(int id)
    {
        try
        {
            return Ok(new {message = "Game retrieved successfully", game = await _gameService.Retrieve(id)});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving game", error = e.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> AddGame([FromBody] GameDto gameDto)
    {
        if (!DtoValidator.ValidateObject(gameDto, out var messages))
        {
            return BadRequest(messages);
        }

        try
        {
            await _gameService.AddGame(gameDto);
            return Ok(new { message = "Game added successfully" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in adding game", error = e.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "User")]
    [Route("after-game-statistics/{gameType}")]
    public async Task<IActionResult> AddAfterGameStatistics([FromBody] GameDto gameDto, string gameType)
    {
        if (!DtoValidator.ValidateObject(gameDto, out var messages))
        {
            return BadRequest(messages);
        }

        try
        {
            var token = GetTokenFromRequest(HttpContext);
            await _gameService.AddAfterGameStatistics(gameDto, gameType, token);
            return Ok(new { message = "Game statistics added successfully" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in adding game statistics", error = e.Message });
        }
    }
    
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditGame(int id, [FromBody] EditGameDto editGameDto)
    {
        if(!DtoValidator.ValidateObject(editGameDto, out var messages))
        {
            return BadRequest(messages);
        }
        try
        {
            return Ok(new { message = "Game edited successfully", game = await _gameService.EditGame(id, editGameDto) });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in editing game", error = e.Message });
        }
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        try
        {
            await _gameService.DeleteGame(id);
            return Ok(new { message = "Game deleted successfully"});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in deleting game", error = e.Message });
        }
    }
    
    private static string GetTokenFromRequest(HttpContext context)
    {
        var retrievedToken = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        if(IsNullOrEmpty(retrievedToken))
        {
            throw new Exception("Token not found");
        }

        return retrievedToken;
    }
}