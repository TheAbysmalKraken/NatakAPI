using Catan.API.Controllers.Models;
using Catan.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Catan.API.Controllers;

[ApiController]
[Route("api/catan")]
public class CatanController(ILogger<CatanController> logger) : ControllerBase
{
    private static readonly List<CatanGame> currentGames = [];

    private readonly ILogger<CatanController> _logger = logger;

    [HttpGet("{gameId}/{playerColour}")]
    public IActionResult GetGameStatus(string gameId, int playerColour)
    {
        var game = currentGames.FirstOrDefault(g => g.Id == gameId);

        if (game is null)
        {
            return NotFound();
        }

        if (playerColour < 0 || playerColour >= game.PlayerCount)
        {
            return BadRequest("Invalid 'playerId'.");
        }

        try
        {
            var response = PlayerSpecificGameStatusResponse.FromDomain(game, playerColour);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost]
    public IActionResult CreateNewGame(
        [FromBody] CreateNewGameRequest request)
    {
        if (request.PlayerCount is null || request.PlayerCount < 3 || request.PlayerCount > 4)
        {
            return BadRequest("A 'playerCount' of 3 or 4 is required.");
        }

        try
        {
            var newGame = new CatanGame(request.PlayerCount.Value);
            currentGames.Add(newGame);

            return Ok(newGame.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }
}