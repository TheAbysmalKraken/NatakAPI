using Catan.API.Requests;
using Catan.Core.GameActions.BuildRoad;
using Catan.Core.GameActions.CreateGame;
using Catan.Core.GameActions.EndTurn;
using Catan.Core.GameActions.GetGame;
using Catan.Core.GameActions.RollDice;
using MediatR;

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
        builder.MapPost("", CreateGameAsync);
        builder.MapPost("{gameId}/roll", RollDiceAsync);
        builder.MapPost("{gameId}/end-turn", EndTurnAsync);
        builder.MapPost("{gameId}/build/road", BuildRoadAsync);

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
}
