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

    [HttpPost("{gameId}/steal-resource")]
    public IActionResult StealResource(string gameId, [FromBody] StealResourceRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.PlayerColourToStealFrom);

        try
        {
            var stealResourceResult = gameManager.StealResource(gameId, request.PlayerColourToStealFrom.Value);

            if (stealResourceResult.IsFailure)
            {
                return StatusCode((int)stealResourceResult.Error.StatusCode, stealResourceResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/{playerColour}/discard-resources")]
    public IActionResult DiscardResources(string gameId, int playerColour, [FromBody] DiscardResourcesRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Resources);

        try
        {
            var discardResourcesResult = gameManager.DiscardResources(gameId, playerColour, request.Resources);

            if (discardResourcesResult.IsFailure)
            {
                return StatusCode((int)discardResourcesResult.Error.StatusCode, discardResourcesResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/trade-with-bank")]
    public IActionResult TradeWithBank(string gameId, [FromBody] TradeWithBankRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.ResourceToGive);
        ArgumentNullException.ThrowIfNull(request.ResourceToReceive);

        try
        {
            var tradeWithBankResult = gameManager.TradeWithBank(gameId, request.ResourceToGive.Value, request.ResourceToReceive.Value);

            if (tradeWithBankResult.IsFailure)
            {
                return StatusCode((int)tradeWithBankResult.Error.StatusCode, tradeWithBankResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

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