using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.CreateGame;

internal sealed class CreateGameCommandHandler(IActiveGameCache cache) :
    ICommandHandler<CreateGameCommand, CreateGameResponse>
{
    public async Task<Result<CreateGameResponse>> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        if (request.PlayerCount < 3 || request.PlayerCount > 4)
        {
            return Result.Failure<CreateGameResponse>(GameErrors.InvalidPlayerCount);
        }

        var newGame = new Game(request.PlayerCount);

        await cache.UpsetAsync(
            newGame.Id,
            newGame,
            cancellationToken: cancellationToken);

        return Result.Success(new CreateGameResponse()
        {
            GameId = newGame.Id
        });
    }
}
