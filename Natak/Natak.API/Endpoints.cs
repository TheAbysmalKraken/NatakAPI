using Natak.API.Requests;
using Natak.Core.GameActions.BuildCity;
using Natak.Core.GameActions.BuildRoad;
using Natak.Core.GameActions.BuildSettlement;
using Natak.Core.GameActions.BuyDevelopmentCard;
using Natak.Core.GameActions.CancelTradeOffer;
using Natak.Core.GameActions.CreateGame;
using Natak.Core.GameActions.DiscardResources;
using Natak.Core.GameActions.EmbargoPlayer;
using Natak.Core.GameActions.EndTurn;
using Natak.Core.GameActions.GetAvailableCityLocations;
using Natak.Core.GameActions.GetAvailableRoadLocations;
using Natak.Core.GameActions.GetAvailableSettlementLocations;
using Natak.Core.GameActions.GetGame;
using Natak.Core.GameActions.MakeTradeOffer;
using Natak.Core.GameActions.MoveRobber;
using Natak.Core.GameActions.PlayKnightCard;
using Natak.Core.GameActions.PlayMonopolyCard;
using Natak.Core.GameActions.PlayRoadBuildingCard;
using Natak.Core.GameActions.PlayYearOfPlentyCard;
using Natak.Core.GameActions.RemoveEmbargo;
using Natak.Core.GameActions.RespondToTradeOffer;
using Natak.Core.GameActions.RollDice;
using Natak.Core.GameActions.StealResource;
using Natak.Core.GameActions.TradeWithBank;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Natak.API;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("api/natak/")
            .WithTags("Natak")
            .WithOpenApi()
            .MapEndpointsToBuilder();
    }

    private static RouteGroupBuilder MapEndpointsToBuilder(
        this RouteGroupBuilder builder)
    {
        builder.MapGet("{gameId}/{playerColour}", GetGameStatusAsync);
        builder.MapGet("{gameId}/available-settlement-locations", GetAvailableSettlementLocationsAsync);
        builder.MapGet("{gameId}/available-city-locations", GetAvailableCityLocationsAsync);
        builder.MapGet("{gameId}/available-road-locations", GetAvailableRoadLocationsAsync);
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
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new PlayRoadBuildingCardCommand(
                gameId);

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
