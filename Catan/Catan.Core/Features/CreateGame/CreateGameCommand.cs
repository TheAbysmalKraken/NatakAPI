namespace Catan.Core.Features.CreateGame;

public sealed record CreateGameCommand(
    int PlayerCount,
    int? Seed = null) : ICommand<CreateGameResponse>;
