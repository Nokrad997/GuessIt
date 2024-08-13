using System.Diagnostics;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly GameService _gameService;
    
    public GameController(GameService gameService)
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
            return BadRequest(new { message = e.Message });
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
            return BadRequest(new { message = e.Message });
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
            var addedGame = await _gameService.AddGame(gameDto);
            return Ok(new { message = "Game added successfully", game = addedGame });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message, innerException = e.InnerException?.Message });
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
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        try
        {
            return Ok(new { message = "Game deleted successfully", game = await _gameService.DeleteGame(id) });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}