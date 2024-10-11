using Natak.API.Requests;
using Natak.Core.GameActions.BuildTown;
using Natak.Core.GameActions.BuildRoad;
using Natak.Core.GameActions.BuildVillage;
using Natak.Core.GameActions.BuyGrowthCard;
using Natak.Core.GameActions.CancelTradeOffer;
using Natak.Core.GameActions.CreateGame;
using Natak.Core.GameActions.DiscardResources;
using Natak.Core.GameActions.EmbargoPlayer;
using Natak.Core.GameActions.EndTurn;
using Natak.Core.GameActions.GetAvailableTownLocations;
using Natak.Core.GameActions.GetAvailableRoadLocations;
using Natak.Core.GameActions.GetAvailableVillageLocations;
using Natak.Core.GameActions.GetGame;
using Natak.Core.GameActions.MakeTradeOffer;
using Natak.Core.GameActions.MoveThief;
using Natak.Core.GameActions.PlaySoldierCard;
using Natak.Core.GameActions.PlayGathererCard;
using Natak.Core.GameActions.PlayRoamingCard;
using Natak.Core.GameActions.PlayWealthCard;
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
    private const int LatestApiVersion = 1;
    
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup($"api/v{LatestApiVersion}/natak/")
            .WithTags("Natak")
            .WithOpenApi()
            .MapEndpointsToBuilder();
    }

    private static RouteGroupBuilder MapEndpointsToBuilder(
        this RouteGroupBuilder builder)
    {
        builder.MapGet("{gameId}/{playerColour}", GetGameStatusAsync);
        builder.MapGet("{gameId}/available-village-locations", GetAvailableVillageLocationsAsync);
        builder.MapGet("{gameId}/available-town-locations", GetAvailableTownLocationsAsync);
        builder.MapGet("{gameId}/available-road-locations", GetAvailableRoadLocationsAsync);
        builder.MapPost("", CreateGameAsync);
        builder.MapPost("{gameId}/roll", RollDiceAsync);
        builder.MapPost("{gameId}/end-turn", EndTurnAsync);
        builder.MapPost("{gameId}/build/road", BuildRoadAsync);
        builder.MapPost("{gameId}/build/village", BuildVillageAsync);
        builder.MapPost("{gameId}/build/town", BuildTownAsync);
        builder.MapPost("{gameId}/buy/growth-card", BuyGrowthCardAsync);
        builder.MapPost("{gameId}/play-growth-card/soldier", PlaySoldierCardAsync);
        builder.MapPost("{gameId}/play-growth-card/roaming", PlayRoamingCardAsync);
        builder.MapPost("{gameId}/play-growth-card/wealth", PlayWealthCardAsync);
        builder.MapPost("{gameId}/play-growth-card/gatherer", PlayGathererCardAsync);
        builder.MapPost("{gameId}/move-thief", MoveThiefAsync);
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

    private static async Task<IResult> GetAvailableVillageLocationsAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetAvailableVillageLocationsQuery(gameId);

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

    private static async Task<IResult> GetAvailableTownLocationsAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetAvailableTownLocationsQuery(gameId);

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

    private static async Task<IResult> BuildVillageAsync(
        ISender sender,
        string gameId,
        BuildBuildingRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new BuildVillageCommand(
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

    private static async Task<IResult> BuildTownAsync(
        ISender sender,
        string gameId,
        BuildBuildingRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new BuildTownCommand(
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

    private static async Task<IResult> BuyGrowthCardAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new BuyGrowthCardCommand(gameId);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> PlaySoldierCardAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new PlaySoldierCardCommand(gameId);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> PlayRoamingCardAsync(
        ISender sender,
        string gameId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new PlayRoamingCardCommand(
                gameId);

            var result = await sender.Send(command, cancellationToken);

            return TypedResultFactory.NoContent(result);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> PlayWealthCardAsync(
        ISender sender,
        string gameId,
        [FromBody] PlayWealthCardRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new PlayWealthCardCommand(
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

    private static async Task<IResult> PlayGathererCardAsync(
        ISender sender,
        string gameId,
        [FromBody] PlayGathererCardRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new PlayGathererCardCommand(
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

    private static async Task<IResult> MoveThiefAsync(
        ISender sender,
        string gameId,
        [FromBody] MoveThiefRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new MoveThiefCommand(
                gameId,
                request.MoveThiefTo.ToPoint());

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
