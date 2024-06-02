using Catan.API.Requests;
using Catan.Application;
using Microsoft.AspNetCore.Mvc;

namespace Catan.API.Controllers;

[ApiController]
[Route("api/catan")]
public class CatanController(
    IGameManager gameManager,
    ILogger<CatanController> logger) : ControllerBase
{
    private readonly ILogger<CatanController> _logger = logger;

    [HttpPost("{gameId}/{playerColour}/embargo-player")]
    public IActionResult EmbargoPlayer(string gameId, int playerColour, [FromBody] EmbargoPlayerRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.PlayerColourToEmbargo);

        try
        {
            var embargoPlayerResult = gameManager.EmbargoPlayer(gameId, playerColour, request.PlayerColourToEmbargo.Value);

            if (embargoPlayerResult.IsFailure)
            {
                return StatusCode((int)embargoPlayerResult.Error.StatusCode, embargoPlayerResult.Error.Message);
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