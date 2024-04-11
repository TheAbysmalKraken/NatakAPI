using Catan.API.Requests;
using Catan.Core.Features.CreateGame;
using Catan.Core.Features.GetGame;
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

            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.Content(result.Error.Message, statusCode: (int)result.Error.StatusCode);
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

            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.Content(result.Error.Message, statusCode: (int)result.Error.StatusCode);
        }
        catch
        {
            return Results.Problem("An error occurred", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
