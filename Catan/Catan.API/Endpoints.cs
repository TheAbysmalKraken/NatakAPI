using Catan.API.Requests;
using Catan.Core.GameActions.BuildCity;
using Catan.Core.GameActions.BuildRoad;
using Catan.Core.GameActions.BuildSettlement;
using Catan.Core.GameActions.BuyDevelopmentCard;
using Catan.Core.GameActions.CancelTradeOffer;
using Catan.Core.GameActions.CreateGame;
using Catan.Core.GameActions.DiscardResources;
using Catan.Core.GameActions.EmbargoPlayer;
using Catan.Core.GameActions.EndTurn;
using Catan.Core.GameActions.GetAvailableCityLocations;
using Catan.Core.GameActions.GetAvailableRoadLocations;
using Catan.Core.GameActions.GetAvailableSettlementLocations;
using Catan.Core.GameActions.GetGame;
using Catan.Core.GameActions.MakeTradeOffer;
using Catan.Core.GameActions.MoveRobber;
using Catan.Core.GameActions.PlayKnightCard;
using Catan.Core.GameActions.PlayMonopolyCard;
using Catan.Core.GameActions.PlayRoadBuildingCard;
using Catan.Core.GameActions.PlayYearOfPlentyCard;
using Catan.Core.GameActions.RemoveEmbargo;
using Catan.Core.GameActions.RespondToTradeOffer;
using Catan.Core.GameActions.RollDice;
using Catan.Core.GameActions.StealResource;
using Catan.Core.GameActions.TradeWithBank;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catan.API;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("api/catan/")
            .WithTags("Catan")
            .WithOpenApi()
            .MapEndpointsToBuilder();
    }

    private static RouteGroupBuilder MapEndpointsToBuilder(
        this RouteGroupBuilder builder)
    {
        builder.MapGet("{gameId}/{playerColour}", GetGameStatusAsync);
        builder.MapGet("{gameId}/{playerColour}/available-settlement-locations", GetAvailableSettlementLocationsAsync);
        builder.MapGet("{gameId}/{playerColour}/available-city-locations", GetAvailableCityLocationsAsync);
        builder.MapGet("{gameId}/{playerColour}/available-road-locations", GetAvailableRoadLocationsAsync);
        builder.MapPost("", CreateGameAsync);
        builder.MapPost("{gameId}/roll", RollDiceAsync);
        builder.MapPost("{gameId}/end-turn", EndTurnAsync);
        builder.MapPost("{gameId}/build/road", BuildRoadAsync);
        builder.MapPost("{gameId}/build/settlement", BuildSettlementAsync);
        builder.MapPost("{gameId}/build/city", BuildCityAsync);
        builder.MapPost("{gameId}/buy/development-card", BuyDevelopmentCardAsync);
        builder.MapPost("{gameId}/play-development-card/knight", PlayKnightCardAsync);
        builder.MapPost("{gameId}/play-development-card/road-building", PlayRoadBuildingCardAsync);
        builder.MapPost("{gameId}/play-development-card/year-of-plenty", PlayYearOfPlentyCardAsync);
        builder.MapPost("{gameId}/play-development-card/monopoly", PlayMonopolyCardAsync);
        builder.MapPost("{gameId}/move-robber", MoveRobberAsync);
        builder.MapPost("{gameId}/steal-resource", StealResourceAsync);
        builder.MapPost("{gameId}/{playerColour}/discard-resources", DiscardResourcesAsync);
        builder.MapPost("{gameId}/trade/bank", TradeWithBankAsync);
        builder.MapPost("{gameId}/{playerColour}/embargo-player", EmbargoPlayerAsync);
        builder.MapPost("{gameId}/{playerColour}/remove-embargo", RemoveEmbargoAsync);
        builder.MapPost("{gameId}/trade/player", MakeTradeOfferAsync);
        builder.MapPost("{gameId}/trade/player/{accept}", RespondToTradeOfferAsync);
        builder.MapPost("{gameId}/trade/player/cancel", CancelTradeOfferAsync);

        return builder;
    }

    private static async Task<IResult> GetGameStatusAsync(
        ISender sender,
        string gameId,
        int playerColour,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetGameQuery(gameId, playerColour);

            var result = await sender.Send(query, cancellationToken);

            return TypedResultFactory.Ok(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> GetAvailableSettlementLocationsAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetAvailableSettlementLocationsQuery(gameId);

            var result = await sender.Send(query, cancellationToken);

            return TypedResultFactory.Ok(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> GetAvailableRoadLocationsAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetAvailableRoadLocationsQuery(gameId);

            var result = await sender.Send(query, cancellationToken);

            return TypedResultFactory.Ok(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> GetAvailableCityLocationsAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetAvailableCityLocationsQuery(gameId);

            var result = await sender.Send(query, cancellationToken);

            return TypedResultFactory.Ok(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> CreateGameAsync(
        ISender sender,
        CreateNewGameRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateGameCommand(request.PlayerCount, request.Seed);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.Ok(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> RollDiceAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new RollDiceCommand(gameId);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.Ok(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> EndTurnAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new EndTurnCommand(gameId);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> BuildRoadAsync(
        ISender sender,
        string gameId,
        BuildRoadRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new BuildRoadCommand(
                gameId,
                request.FirstPoint.ToPoint(),
                request.SecondPoint.ToPoint());

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> BuildSettlementAsync(
        ISender sender,
        string gameId,
        BuildBuildingRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new BuildSettlementCommand(
                gameId,
                request.Point.ToPoint());

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> BuildCityAsync(
        ISender sender,
        string gameId,
        BuildBuildingRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new BuildCityCommand(
                gameId,
                request.Point.ToPoint());

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> BuyDevelopmentCardAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new BuyDevelopmentCardCommand(gameId);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> PlayKnightCardAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new PlayKnightCardCommand(gameId);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> PlayRoadBuildingCardAsync(
        ISender sender,
        string gameId,
        [FromBody] PlayRoadBuildingCardRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new PlayRoadBuildingCardCommand(
                gameId,
                request.FirstRoadFirstPoint.ToPoint(),
                request.FirstRoadSecondPoint.ToPoint(),
                request.SecondRoadFirstPoint.ToPoint(),
                request.SecondRoadSecondPoint.ToPoint());

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> PlayYearOfPlentyCardAsync(
        ISender sender,
        string gameId,
        [FromBody] PlayYearOfPlentyCardRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new PlayYearOfPlentyCardCommand(
                gameId,
                request.FirstResource,
                request.SecondResource);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> PlayMonopolyCardAsync(
        ISender sender,
        string gameId,
        [FromBody] PlayMonopolyCardRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new PlayMonopolyCardCommand(
                gameId,
                request.Resource);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> MoveRobberAsync(
        ISender sender,
        string gameId,
        [FromBody] MoveRobberRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new MoveRobberCommand(
                gameId,
                request.MoveRobberTo.ToPoint());

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> StealResourceAsync(
        ISender sender,
        string gameId,
        [FromBody] StealResourceRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new StealResourceCommand(
                gameId,
                request.VictimColour);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> DiscardResourcesAsync(
        ISender sender,
        string gameId,
        int playerColour,
        [FromBody] DiscardResourcesRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DiscardResourcesCommand(
                gameId,
                playerColour,
                request.Resources);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> TradeWithBankAsync(
        ISender sender,
        string gameId,
        [FromBody] TradeWithBankRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new TradeWithBankCommand(
                gameId,
                request.ResourceToGive,
                request.ResourceToGet);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> EmbargoPlayerAsync(
        ISender sender,
        string gameId,
        int playerColour,
        [FromBody] EmbargoPlayerRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new EmbargoPlayerCommand(
                gameId,
                playerColour,
                request.PlayerColourToEmbargo);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> RemoveEmbargoAsync(
        ISender sender,
        string gameId,
        int playerColour,
        [FromBody] RemoveEmbargoRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new RemoveEmbargoCommand(
                gameId,
                playerColour,
                request.PlayerColourToRemoveEmbargoOn);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> MakeTradeOfferAsync(
        ISender sender,
        string gameId,
        [FromBody] MakeTradeOfferRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new MakeTradeOfferCommand(
                gameId,
                request.Offer,
                request.Request);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> RespondToTradeOfferAsync(
        ISender sender,
        string gameId,
        int playerColour,
        bool accept,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new RespondToTradeOfferCommand(
                gameId,
                playerColour,
                accept);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> CancelTradeOfferAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CancelTradeOfferCommand(gameId);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
