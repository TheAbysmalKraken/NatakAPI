using Catan.API.Controllers.Models;
using Catan.Application;
using Catan.Core.Features.GetGame;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catan.API.Controllers;

[ApiController]
[Route("api/catan")]
public class CatanController(
    ISender sender,
    IGameManager gameManager,
    ILogger<CatanController> logger) : ControllerBase
{
    private readonly ILogger<CatanController> _logger = logger;

    [HttpGet("{gameId}/{playerColour}")]
    public async Task<IActionResult> GetGameStatus(string gameId, int playerColour)
    {
        try
        {
            var gameResult = await sender.Send(new GetGameQuery(gameId, playerColour));

            if (gameResult.IsFailure)
            {
                return StatusCode((int)gameResult.Error.StatusCode, gameResult.Error.Message);
            }

            return Ok(gameResult.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Error}", ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost]
    public IActionResult CreateNewGame(
        [FromBody] CreateNewGameRequest request,
        [FromQuery] int? seed = null)
    {
        if (request.PlayerCount is null)
        {
            return BadRequest("Player count is required");
        }

        try
        {
            var newGameResult = gameManager.CreateNewGame(request.PlayerCount.Value, seed);

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

    [HttpPost("{gameId}/build/road")]
    public IActionResult BuildRoad(string gameId, [FromBody] BuildRoadRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.FirstX);
        ArgumentNullException.ThrowIfNull(request.FirstY);
        ArgumentNullException.ThrowIfNull(request.SecondX);
        ArgumentNullException.ThrowIfNull(request.SecondY);

        try
        {
            var buildRoadResult = gameManager.BuildRoad(gameId, request.FirstX.Value, request.FirstY.Value, request.SecondX.Value, request.SecondY.Value);

            if (buildRoadResult.IsFailure)
            {
                return StatusCode((int)buildRoadResult.Error.StatusCode, buildRoadResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/build/settlement")]
    public IActionResult BuildSettlement(string gameId, [FromBody] BuildBuildingRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.X);
        ArgumentNullException.ThrowIfNull(request.Y);

        try
        {
            var buildSettlementResult = gameManager.BuildSettlement(gameId, request.X.Value, request.Y.Value);

            if (buildSettlementResult.IsFailure)
            {
                return StatusCode((int)buildSettlementResult.Error.StatusCode, buildSettlementResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/build/city")]
    public IActionResult BuildCity(string gameId, [FromBody] BuildBuildingRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.X);
        ArgumentNullException.ThrowIfNull(request.Y);

        try
        {
            var buildCityResult = gameManager.BuildCity(gameId, request.X.Value, request.Y.Value);

            if (buildCityResult.IsFailure)
            {
                return StatusCode((int)buildCityResult.Error.StatusCode, buildCityResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/buy/development-card")]
    public IActionResult BuyDevelopmentCard(string gameId)
    {
        try
        {
            var buyDevelopmentCardResult = gameManager.BuyDevelopmentCard(gameId);

            if (buyDevelopmentCardResult.IsFailure)
            {
                return StatusCode((int)buyDevelopmentCardResult.Error.StatusCode, buyDevelopmentCardResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/play-development-card/knight")]
    public IActionResult PlayKnightCard(string gameId, [FromBody] PlayKnightCardRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.X);
        ArgumentNullException.ThrowIfNull(request.Y);
        ArgumentNullException.ThrowIfNull(request.PlayerColourToStealFrom);

        try
        {
            var playKnightCardResult = gameManager.PlayKnightCard(gameId, request.X.Value, request.Y.Value, request.PlayerColourToStealFrom.Value);

            if (playKnightCardResult.IsFailure)
            {
                return StatusCode((int)playKnightCardResult.Error.StatusCode, playKnightCardResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/play-development-card/road-building")]
    public IActionResult PlayRoadBuildingCard(string gameId, [FromBody] PlayRoadBuildingCardRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.FirstX);
        ArgumentNullException.ThrowIfNull(request.FirstY);
        ArgumentNullException.ThrowIfNull(request.SecondX);
        ArgumentNullException.ThrowIfNull(request.SecondY);
        ArgumentNullException.ThrowIfNull(request.ThirdX);
        ArgumentNullException.ThrowIfNull(request.ThirdY);
        ArgumentNullException.ThrowIfNull(request.FourthX);
        ArgumentNullException.ThrowIfNull(request.FourthY);

        try
        {
            var playRoadBuildingCardResult = gameManager.PlayRoadBuildingCard(
                gameId,
                request.FirstX.Value,
                request.FirstY.Value,
                request.SecondX.Value,
                request.SecondY.Value,
                request.ThirdX.Value,
                request.ThirdY.Value,
                request.FourthX.Value,
                request.FourthY.Value);

            if (playRoadBuildingCardResult.IsFailure)
            {
                return StatusCode((int)playRoadBuildingCardResult.Error.StatusCode, playRoadBuildingCardResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/play-development-card/year-of-plenty")]
    public IActionResult PlayYearOfPlentyCard(string gameId, [FromBody] PlayYearOfPlentyCardRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.FirstResourceType);
        ArgumentNullException.ThrowIfNull(request.SecondResourceType);

        try
        {
            var playYearOfPlentyCardResult = gameManager.PlayYearOfPlentyCard(gameId, request.FirstResourceType.Value, request.SecondResourceType.Value);

            if (playYearOfPlentyCardResult.IsFailure)
            {
                return StatusCode((int)playYearOfPlentyCardResult.Error.StatusCode, playYearOfPlentyCardResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/play-development-card/monopoly")]
    public IActionResult PlayMonopolyCard(string gameId, [FromBody] PlayMonopolyCardRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.ResourceType);

        try
        {
            var playMonopolyCardResult = gameManager.PlayMonopolyCard(gameId, request.ResourceType.Value);

            if (playMonopolyCardResult.IsFailure)
            {
                return StatusCode((int)playMonopolyCardResult.Error.StatusCode, playMonopolyCardResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost("{gameId}/move-robber")]
    public IActionResult MoveRobber(string gameId, [FromBody] MoveRobberRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.X);
        ArgumentNullException.ThrowIfNull(request.Y);

        try
        {
            var moveRobberResult = gameManager.MoveRobber(gameId, request.X.Value, request.Y.Value);

            if (moveRobberResult.IsFailure)
            {
                return StatusCode((int)moveRobberResult.Error.StatusCode, moveRobberResult.Error.Message);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(500);
        }
    }

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