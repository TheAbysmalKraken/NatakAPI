using Catan.API.Controllers.Models;
using Catan.Application;
using Microsoft.AspNetCore.Mvc;

namespace Catan.API.Controllers;

[ApiController]
[Route("api/catan")]
public class CatanController(ILogger<CatanController> logger) : ControllerBase
{
    private static readonly CatanGameManager gameManager = new();

    private readonly ILogger<CatanController> _logger = logger;

    [HttpGet("{gameId}/{playerColour}")]
    public IActionResult GetGameStatus(string gameId, int playerColour)
    {
        try
        {
            var gameResult = gameManager.GetGameStatus(gameId, playerColour);

            if (gameResult.IsFailure)
            {
                return StatusCode((int)gameResult.Error.StatusCode, gameResult.Error.Message);
            }

            return Ok(gameResult.Value);
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
        ArgumentNullException.ThrowIfNull(request.PlayerCount);

        try
        {
            var newGameResult = gameManager.CreateNewGame(request.PlayerCount.Value);

            if (newGameResult.IsFailure)
            {
                return StatusCode((int)newGameResult.Error.StatusCode, newGameResult.Error.Message);
            }

            return Ok(newGameResult.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/roll")]
    public IActionResult RollDice(string gameId)
    {
        try
        {
            var rollResult = gameManager.RollDice(gameId);

            if (rollResult.IsFailure)
            {
                return StatusCode((int)rollResult.Error.StatusCode, rollResult.Error.Message);
            }

            return Ok(rollResult.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/end-turn")]
    public IActionResult EndTurn(string gameId)
    {
        try
        {
            var endTurnResult = gameManager.EndTurn(gameId);

            if (endTurnResult.IsFailure)
            {
                return StatusCode((int)endTurnResult.Error.StatusCode, endTurnResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }
}